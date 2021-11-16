using DatabaseServices.Data;
using DatabaseServices.DTO;
using DomainModel;
using System.Threading.Tasks;

namespace DatabaseServices.Services
{
    public interface IPatientsServices
    {
        public bool Ping();
        public Task<PatientDTO> GetPatient(int documentID);
        public Task<Patient> PostPatient(PatientDTO PatientDTO);
        public void SetContext(DatabaseServicesContext context);

    }
}
