using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DatabaseServices.Data;
using DatabaseServices.DTO;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using DatabaseServices.Services;

namespace DatabaseServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicsController : ControllerBase
    {
        private readonly IMedicsServices _services;

        public MedicsController(IMedicsServices medicsServices, DatabaseServicesContext context)
        {
            _services = medicsServices;
            _services.SetContext(context);
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
                var result = _services.GetMedic(UserName);
                if (result == null)
                {
                    return BadRequest(JsonConvert.SerializeObject("Medic with username: " + UserName + " Does Not Exist!"));
                }
                else
                {
                    return Ok(JsonConvert.SerializeObject(result));
                }
            }
            catch (Exception)
            {
                return BadRequest(JsonConvert.SerializeObject("There was an error, try again later!"));
                throw;
            }
        }

        // PUT: api/Medics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{UserName}")]
        public async Task<IActionResult> PutMedic(String UserName , MedicDTO medic)
        {

            try
            {
                var result = await _services.PutMedic(UserName, medic);
                if (result.Equals("Mismatch"))
                {
                    return BadRequest("Wrong Request, Try again");
                }
                else if (result.Equals("NotFound"))
                {
                    return NotFound(JsonConvert.SerializeObject("Medic with username: " + UserName + "Does Not Exist!"));
                }
                else
                {
                    return Ok(JsonConvert.SerializeObject("Updated Data"));
                }

            }
            catch (Exception)
            {
                return BadRequest("There was a problem, try again later!");
                throw;
            }
        }
    }
}
