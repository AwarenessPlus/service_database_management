using DatabaseServices.Data;
using DatabaseServices.DTO;
using DomainModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseServices.Services
{
    public class ProceduresServices : IProceduresServices
    {
        private DatabaseServicesContext _context;
        private IWebHostEnvironment _environment;

        public void SetContextAndIEnvironment(DatabaseServicesContext context, IWebHostEnvironment environment)
        {
            if (_context == null)
            {
                _context = context;
            }
            if (_environment == null)
            {
                _environment = environment;
            }
        }

        public bool Ping()
        {
            return true;
        }
        public async Task<List<ProcedureDTO>> GetProceduresByMedic(int MedicID)
        {
            var proceduresList = await _context.Procedure.Where(e => e.MedicID == MedicID).ToListAsync();
            Console.Write(proceduresList);
            List<ProcedureDTO> ProcedureDTOList = new();
            foreach (var item in proceduresList)
            {
                var patient = await _context.Patient.FirstAsync(e => e.PatientID == item.PacientID);
                var user = await _context.User.FirstAsync(e => e.UserID == patient.UserID);
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
                procedureAux.ProcedureID = item.ProcedureID;
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
                return ProcedureDTOList;
            }
            else
            {
                return null;
            }
        }
        public async Task<int> PostProcedure(ProcedureDTO procedure)
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

            return procedure1.ProcedureID;
        }
        public async Task<bool> DeleteProcedure(int id)
        {
            var procedure = await _context.Procedure.FindAsync(id);

            if (procedure == null)
            {
                return false;
            }

            if (_context.VideoFile.Any(e => e.ProcedureID == id))
            {
                var videoFile = await _context.VideoFile.FirstAsync(e => e.ProcedureID == id);
                System.IO.File.Delete(videoFile.Filepath);
                _context.VideoFile.Remove(videoFile);
                await _context.SaveChangesAsync();
            }

            _context.Procedure.Remove(procedure);

            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<string> UploadVideo(IFormFile file, int procedureID)
        {
            if (file == null)
            {
                return "NoFileDataInRequest";
            }

            if (!ProcedureExists(procedureID))
            {
                return "ProcedureNotExist";
            }
            String[] filedata = file.FileName.Split(".");

            if (filedata[1] != "mp4" && !ProcedureExists(procedureID))
            {
                return "WrongFileFormat";
            }
            var videoquery = await GetVideo(procedureID);
            if (videoquery != null)
            {
                return "VideoAlreadyUploaded";
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
            Stream fileStream = new FileStream(video.Filepath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            await _context.VideoFile.AddAsync(video);

            await _context.SaveChangesAsync();
            fileStream.Dispose();
            fileStream.Close();
            

            return "Done";
        }
        public async Task<FileStream> GetVideo(int procedureID)
        {
            try
            {
                var videoData = await _context.VideoFile.FirstAsync(e => e.ProcedureID == procedureID);

                var videoFileData = File.OpenRead(videoData.Filepath);
                return videoFileData;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public bool ProcedureExists(int id)
        {
            return _context.Procedure.Any(e => e.ProcedureID == id);
        }
    }
}
