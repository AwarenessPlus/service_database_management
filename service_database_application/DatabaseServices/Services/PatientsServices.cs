using DatabaseServices.Data;
using DatabaseServices.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainModel;

namespace DatabaseServices.Services
{
    public class PatientsServices : IPatientsServices
    {

        private DatabaseServicesContext _context;

        public void SetContext(DatabaseServicesContext context)
        {
            if(_context == null)
            {
                _context = context;
            }
        }

        public bool Ping()
        {
            return true;
        }

        public async Task<PatientDTO> GetPatient(int documentID)
        {
            try
            {
                var Patient = await _context.Patient.FirstAsync(e => e.DocumentID == documentID);
                var user = await _context.User.FindAsync(Patient.UserID);
                PatientDTO PatientDTO = new();
                PatientDTO.FirstName = user.FirstName + " " + user.SecondName;
                PatientDTO.LastName = user.Surname + " " + user.LastName;
                PatientDTO.BloodGroup = Patient.Bloodgroup.Value.ToString();
                PatientDTO.Rh = Patient.Rh.Value.ToString();
                PatientDTO.Sex = Patient.Sex.Value.ToString();
                PatientDTO.DocumentID = Patient.DocumentID;
                PatientDTO.BirthDate = user.BirthDate;
                var age = DateTime.Now.Year - user.BirthDate.Year;
                if (DateTime.Now.DayOfYear < user.BirthDate.DayOfYear)
                {
                    age -= 1;
                }
                PatientDTO.PatientAge = age;

                return PatientDTO;

            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<Patient> PostPatient(PatientDTO PatientDTO)
        {
            Patient Patient = new();
            User user = new();
            String[] firstName = PatientDTO.FirstName.Split(' ');
            string[] lastName = PatientDTO.LastName.Split(' ');
            user.FirstName = firstName[0];
            user.SecondName = firstName[1];
            user.Surname = lastName[0];
            user.LastName = lastName[1];
            user.BirthDate = PatientDTO.BirthDate;
            Patient.PatientInfo = user;
            Patient.Rh = Enum.Parse<Rh>(PatientDTO.Rh);
            Patient.Bloodgroup = Enum.Parse<BloodGroup>(PatientDTO.BloodGroup);
            Patient.Sex = Enum.Parse<Sex>(PatientDTO.Sex);
            Patient.DocumentID = PatientDTO.DocumentID;
            _context.Patient.Add(Patient);
            await _context.SaveChangesAsync();

            return Patient;
        }
    }
}
