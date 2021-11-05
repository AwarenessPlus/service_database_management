using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseServices.Data;
using DomainModel;
using DatabaseServices.DTO;
using Newtonsoft.Json;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace DatabaseServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProceduresController : ControllerBase
    {
        private readonly DatabaseServicesContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProceduresController(DatabaseServicesContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [HttpGet("ping")]
        public ActionResult GetPing()
        {
            return Ok(JsonConvert.SerializeObject("Ping"));
        }

        [HttpGet("{MedicID}")]
        public ActionResult<IEnumerable<Procedure>> GetProceduresByMedic(int MedicID)
        {
            var proceduresList = _context.Procedure.Where(e => e.MedicID == MedicID);
            List<ProcedureDTO> ProcedureDTOList = new();
            foreach (var item in proceduresList)
            {
                var patient = _context.Patient.First(e => e.PatientID == item.PacientID);
                var user = _context.User.First(e => e.UserID == patient.UserID);
                ProcedureDTO procedureAux = new();
                PatientDTO patientAux = new();
                patientAux.FirstName = user.FirstName + " " + user.SecondName;
                patientAux.LastName = user.Surname + " " + user.LastName;
                var age = DateTime.Now.Year - user.BirthDate.Year;
                if (DateTime.Now.DayOfYear < user.BirthDate.DayOfYear)
                {
                    age -= 1;
                }
                patientAux.BirthDate = user.BirthDate;
                patientAux.Sex = patient.Sex.Value.ToString();
                patientAux.BloodGroup = patient.Bloodgroup.Value.ToString();
                patientAux.Rh = patient.Rh.Value.ToString();
                patientAux.DocumentID = patient.DocumentID;
                patientAux.PatientID = patient.PatientID;
                procedureAux.MedicID = item.MedicID;
                procedureAux.ProcedureName = item.ProcedureName;
                procedureAux.PatientStatus = item.PatientStatus;
                procedureAux.Asa = item.Asa;
                procedureAux.PatientID = patientAux.PatientID;
                procedureAux.PatientAge = age;
                procedureAux.PatientInfo = patientAux;
                procedureAux.ProcedureDate = item.ProcedureDate;
                ProcedureDTOList.Add(procedureAux);
            }
            if (ProcedureDTOList.Any())
            {
                return Ok(JsonConvert.SerializeObject(ProcedureDTOList));
            }
            else
            {
                return BadRequest(JsonConvert.SerializeObject("Not procedures Exist for medic: " + MedicID));
            }
        }

        // POST: api/Procedures
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("new-procedure")]
        public async Task<ActionResult<Procedure>> PostProcedure(ProcedureDTO procedure)
        {
            Procedure procedure1 = new();

            if (_context.Patient.Any(e => e.DocumentID == procedure.PatientInfo.DocumentID))
            {
                var existPatient = await _context.Patient.FirstAsync(e => e.DocumentID == procedure.PatientInfo.DocumentID);
                procedure1.Pacient = existPatient;
            }
            else
            {
                Patient Patient = new();
                User user = new();
                String[] firstName = procedure.PatientInfo.FirstName.Split(' ');
                string[] lastName = procedure.PatientInfo.LastName.Split(' ');
                user.FirstName = firstName[0];
                user.SecondName = firstName[1];
                user.Surname = lastName[0];
                user.LastName = lastName[1];
                user.BirthDate = procedure.PatientInfo.BirthDate;
                Patient.PatientInfo = user;
                Patient.Rh = Enum.Parse<Rh>(procedure.PatientInfo.Rh);
                Patient.Bloodgroup = Enum.Parse<BloodGroup>(procedure.PatientInfo.BloodGroup);
                Patient.Sex = Enum.Parse<Sex>(procedure.PatientInfo.Sex);
                Patient.DocumentID = procedure.PatientInfo.DocumentID;
                _context.User.Add(user);
                _context.Patient.Add(Patient);

                procedure1.Pacient = Patient;

            }

            procedure1.MedicID = procedure.MedicID;
            procedure1.ProcedureName = procedure.ProcedureName;
            procedure1.PatientStatus = procedure.PatientStatus;
            procedure1.Asa = procedure.Asa;
            procedure1.ProcedureDate = DateTime.Now;

            _context.Procedure.Add(procedure1);
           
            await _context.SaveChangesAsync();

            return Ok("Procedure Created");
        }

        // DELETE: api/Procedures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProcedure(int id)
        {
            var procedure = await _context.Procedure.FindAsync(id);
            var videoFile = await _context.VideoFile.FirstAsync(e => e.ProcedureID == id);
            if (procedure == null)
            {
                return NotFound(JsonConvert.SerializeObject("Procedure with ID: " + id + " Does Not Exist!" ));
            }

            System.IO.File.Delete(videoFile.Filepath);

            _context.VideoFile.Remove(videoFile);

            await _context.SaveChangesAsync();

            _context.Procedure.Remove(procedure);

            await _context.SaveChangesAsync();

            return Ok(JsonConvert.SerializeObject("Procedure with ID: " + id + " Deleted!"));
        }

        [HttpPost("upload-video/{procedureID}")]
        public async Task<IActionResult> PostVideoFile(IFormFile file, int procedureID)
        {
            String[] filedata = file.FileName.Split(".");

            if (!ProcedureExists(procedureID))
            {
                return BadRequest(JsonConvert.SerializeObject("Procedure With procedureID " + procedureID + " Does Not Exist"));
            }

            if(filedata[1] != "mp4" && !ProcedureExists(procedureID))
            {
                return BadRequest(JsonConvert.SerializeObject("Only Mp4 Files Supported. Check the file and try again"));
            }
            VideoFile video = new();

            video.Filename = file.FileName;
            video.ProcedureID = procedureID;
            string uploads = Path.Combine(_environment.ContentRootPath, "ProcedureVideos");
            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }
            video.Filepath = Path.Combine(uploads, file.FileName);
            using Stream fileStream = new FileStream(video.Filepath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            await _context.VideoFile.AddAsync(video);

            await _context.SaveChangesAsync();

            return Ok(JsonConvert.SerializeObject("Video Published"));
        }

        [HttpGet("get-video/{procedureID}")]
        public async Task<IActionResult> GetVideoFile(int procedureID)
        {
            try
            {
                var videoData = await _context.VideoFile.FirstAsync(e => e.ProcedureID == procedureID);
                var videoFileData = System.IO.File.OpenRead(videoData.Filepath);

                return File(videoFileData, "video/mp4", videoData.Filename);
            }
            catch (Exception)
            {

                return BadRequest(JsonConvert.SerializeObject("Video for Procedure " 
                    + procedureID + " Does Not Exist!"));
            }
        }

        private bool ProcedureExists(int id)
        {
            return _context.Procedure.Any(e => e.ProcedureID == id);
        }
    }
}