using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseServices.Data;
using DomainModel;
using DatabaseServices.DTO;
using Newtonsoft.Json;

namespace DatabaseServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly DatabaseServicesContext _context;

        public PatientsController(DatabaseServicesContext context)
        {
            _context = context;
        }

        [HttpGet("ping")]
        public ActionResult GetPing()
        {
            return Ok(JsonConvert.SerializeObject("Ping"));
        }

        // GET: api/Patients/5
        [HttpGet("{documentID}")]
        public async Task<ActionResult<PatientDTO>> GetPatient(int documentID)
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

                return Ok(JsonConvert.SerializeObject(PatientDTO));

            }
            catch (Exception)
            {
                return BadRequest(JsonConvert.SerializeObject("Patient with Document " + documentID + " doesn't exist" ));
            }
            
        }

        // POST: api/Patients/newPatient
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("new-patient")]
        public async Task<ActionResult<Patient>> PostPatient(PatientDTO PatientDTO)
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

            return CreatedAtAction("GetPatient", new { documentID = Patient.DocumentID }, Patient);
        }
    }
}
