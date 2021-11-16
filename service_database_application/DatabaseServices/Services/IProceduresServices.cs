using DatabaseServices.Data;
using DatabaseServices.DTO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseServices.Services
{
    public interface IProceduresServices
    {
        public bool Ping();
        public void SetContextAndIEnvironment(DatabaseServicesContext context, IWebHostEnvironment environment);
        public Task<List<ProcedureDTO>> GetProceduresByMedic(int MedicID);
        public Task<int> PostProcedure(ProcedureDTO procedure);
        public Task<bool> DeleteProcedure(int procedureID);
        public Task<string> UploadVideo(IFormFile file, int procedureID);
        public Task<FileStream> GetVideo(int procedureID);
        public bool ProcedureExists(int procedureID);
    }
}
