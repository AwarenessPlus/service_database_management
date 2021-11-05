using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseServices.Data;
using DatabaseServices.DTO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace DatabaseServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicsController : ControllerBase
    {
        private readonly DatabaseServicesContext _context;

        public MedicsController(DatabaseServicesContext context)
        {
            _context = context;
        }

        // GET: api/Medics/ping
        [HttpGet("ping")]
        public ActionResult GetPing()
        {
            return Ok(JsonConvert.SerializeObject("Ping"));
        }

        // GET: api/Medics/5
        [HttpGet("{UserName}")]
        public ActionResult GetMedic(String UserName)
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

                return Ok(JsonConvert.SerializeObject(medicInfo));
            }
            catch (Exception)
            {
                return BadRequest(JsonConvert.SerializeObject("Medic with username: " + UserName + "Does Not Exist!"));
            }
        }

        // PUT: api/Medics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{UserName}")]
        public async Task<IActionResult> PutMedic(String UserName , MedicDTO medic)
        {
            if (UserName != medic.Username)
            {
                return BadRequest("Wrong Request, Try again");
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
                    return NotFound(JsonConvert.SerializeObject("Medic with username: " + UserName + "Does Not Exist!"));
                }
                else
                {
                    throw;
                }
            }

            return Ok(JsonConvert.SerializeObject("Updated Data"));
        }

        private bool MedicExists(int id)
        {
            return _context.Medic.Any(e => e.MedicID == id);
        }
    }
}
