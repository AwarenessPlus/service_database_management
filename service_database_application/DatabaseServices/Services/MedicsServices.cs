using DatabaseServices.Data;
using DatabaseServices.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseServices.Services
{
    public class MedicsServices : IMedicsServices
    {
        private DatabaseServicesContext _context;

        public void SetContext(DatabaseServicesContext context)
        {
            if (_context == null)
            {
                _context = context;
            }
        }

        public bool Ping()
        {
            return true;
        }

        public MedicDTO GetMedic(string UserName)
        {
            try
            {
                var auth = _context.Authentication.First(e => e.UserName == UserName);
                var medic = _context.Medic.First(e => e.AuthenticationID == auth.AuthenticationID);
                var user = _context.User.First(e => e.UserID == medic.UserID);

                MedicDTO medicInfo = new();
                medicInfo.FirstName = user.FirstName + " " + user.SecondName;
                medicInfo.LastName = user.Surname + " " + user.LastName;
                var age = DateTime.UtcNow.Year - user.BirthDate.Year;
                if (DateTime.Now.DayOfYear < user.BirthDate.DayOfYear)
                {
                    age -= 1;
                }
                medicInfo.Age = age;
                medicInfo.BirthDate = user.BirthDate;
                medicInfo.MedicID = medic.MedicID;
                medicInfo.Rotation = medic.Rotation;
                medicInfo.Semester = medic.Semester;
                medicInfo.Username = auth.UserName;

                return medicInfo;
            }
            catch (Exception)
            {
                return null;
            }
        }
        public async Task<string> PutMedic(string UserName, MedicDTO medic)
        {
            if (UserName != medic.Username)
            {
                return "Mismatch";
            }
            var auth = _context.Authentication.First(e => e.UserName == medic.Username);
            var medicModifier = _context.Medic.First(e => e.AuthenticationID == auth.AuthenticationID);
            var userModifier = _context.User.First(e => e.UserID == medicModifier.UserID);


            String[] firstName = medic.FirstName.Split(' ');
            string[] lastName = medic.LastName.Split(' ');

            userModifier.FirstName = firstName[0];
            userModifier.SecondName = firstName[1];
            userModifier.Surname = lastName[0];
            userModifier.LastName = lastName[1];
            userModifier.BirthDate = medic.BirthDate;

            medicModifier.Semester = medic.Semester;
            medicModifier.Rotation = medic.Rotation;

            _context.Entry(medicModifier).State = EntityState.Modified;
            _context.Entry(userModifier).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicExists(medic.MedicID))
                {
                    return "NotFound";
                }
                else
                {
                    throw;
                }
            }

            return "Updated";
        }

        public bool MedicExists(int id)
        {
            return _context.Medic.Any(e => e.MedicID == id);
        }
    }
}
