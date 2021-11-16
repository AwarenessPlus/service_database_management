using DatabaseServices.Data;
using DatabaseServices.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatabaseServices.Services
{
    public interface IMedicsServices
    {
        public bool Ping();
        public MedicDTO GetMedic(string UserName);
        public Task<string> PutMedic(string UserName, MedicDTO medic);
        public bool MedicExists(int id);
        public void SetContext(DatabaseServicesContext context);
    }
}
