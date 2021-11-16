using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseServices.Data;
using DomainModel;
using DatabaseServices.DTO;
using Newtonsoft.Json;
using DatabaseServices.Services;

namespace DatabaseServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {
        private readonly IPatientsServices _services;

        public PatientsController(IPatientsServices patientServices, DatabaseServicesContext context)
        {
            _services = patientServices;
            _services.SetContext(context);
        }

        [HttpGet("ping")]
        public ActionResult GetPing()
        {
            return Ok(JsonConvert.SerializeObject("Ping"));
        }

        // GET: api/Patients/5
        [HttpGet("{documentID}")]
        public async Task<ActionResult> GetPatient(int documentID)
        {
            try
            {
                var result = await _services.GetPatient(documentID);
                if (result == null)
                {
                    return BadRequest(JsonConvert.SerializeObject("Patient with Document " + documentID + " doesn't exist"));
                }
                else
                {
                    return Ok(JsonConvert.SerializeObject(result));
                }
            }
            catch (Exception)
            {
                return NotFound(JsonConvert.SerializeObject("There was an error, try again later!"));
                throw;
            }
        }

        // POST: api/Patients/newPatient
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("new-patient")]
        public async Task<ActionResult> PostPatient(PatientDTO PatientDTO)
        {
            try
            {
                var result = await _services.PostPatient(PatientDTO);
                return CreatedAtAction("GetPatient", new { documentID = result.DocumentID }, result);
            }
            catch (Exception)
            {
                return BadRequest(JsonConvert.SerializeObject("There's might be an error while creating new patient, try again!"));
                throw;
            }
        }
    }
}
