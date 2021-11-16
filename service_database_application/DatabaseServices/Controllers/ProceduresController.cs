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
using System.Threading;
using DatabaseServices.Services;

namespace DatabaseServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProceduresController : ControllerBase
    {
        private readonly IProceduresServices _services;

        public ProceduresController(IProceduresServices proceduresServices, DatabaseServicesContext context, IWebHostEnvironment environment)
        {
            _services = proceduresServices;
            _services.SetContextAndIEnvironment(context, environment);
        }

        [HttpGet("ping")]
        public ActionResult GetPing()
        {
            try
            {
                _services.Ping();
                return Ok(JsonConvert.SerializeObject("Ping"));
            }
            catch (Exception)
            {
                return NotFound(JsonConvert.SerializeObject("Ping was unsucessfull"));
                throw;
            }

        }

        [HttpGet("{MedicID}")]
        public async Task<ActionResult> GetProceduresByMedic(int MedicID)
        {
            try
            {
                var result = await _services.GetProceduresByMedic(MedicID);
                if (result != null)
                {
                    return Ok(JsonConvert.SerializeObject(result));
                }
                else
                {
                    return BadRequest(JsonConvert.SerializeObject("Not procedures Exist for medic: " + MedicID));
                }
            }
            catch (Exception)
            {
                return NotFound(JsonConvert.SerializeObject("There is an error during this request, try again later!"));
                throw;
            }
        }

        // POST: api/Procedures
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("new-procedure")]
        public async Task<ActionResult> PostProcedure(ProcedureDTO procedure)
        {
            try
            {
                var result = await _services.PostProcedure(procedure);
                return Ok(JsonConvert.SerializeObject(result));
            }
            catch (Exception)
            {
                return BadRequest("Wrong or missing data in request, try again!");
                throw;
            }
        }

        // DELETE: api/Procedures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProcedure(int id)
        {
            try
            {
                var result = await _services.DeleteProcedure(id);
                if (result)
                {
                    return Ok(JsonConvert.SerializeObject("Procedure with ID: " + id + " Deleted!"));
                }
                else
                {
                    return NotFound(JsonConvert.SerializeObject("Procedure with ID: " + id + " Does Not Exist!"));
                }
            }
            catch (Exception)
            {
                return BadRequest("Wrong or missing data in request, try again!");
                throw;
            }
        }

        [HttpPost("upload-video/{procedureID}")]
        [DisableRequestSizeLimit()]
        public async Task<ActionResult> PostVideoFile(IFormFile file, int procedureID)
        {
            try
            {
                var result = await _services.UploadVideo(file, procedureID);
                if (result.Equals("WrongFileFormat"))
                {
                    return BadRequest(JsonConvert.SerializeObject("Only Mp4 Files Supported. Check the file and try again"));
                }
                else if (result.Equals("NoFileDataInRequest"))
                {
                    return BadRequest(JsonConvert.SerializeObject("There is no video in request!"));
                }
                else if (result.Equals("ProcedureNotExist"))
                {
                    return BadRequest(JsonConvert.SerializeObject("Procedure With procedureID " + procedureID + " Does Not Exist"));
                }
                else if (result.Equals("VideoAlreadyUploaded"))
                {
                    return BadRequest(JsonConvert.SerializeObject("This procedure has a video uploaded!"));
                }
                else
                {
                    return Ok(JsonConvert.SerializeObject("Video Published"));
                }
            }
            catch (Exception e)
            {
                return NotFound(JsonConvert.SerializeObject("There was an error while updating video, try again later! " + e.ToString()));
                throw;
            }
        }

        [HttpGet("get-video/{procedureID}")]
        public async Task<IActionResult> GetVideoFile(int procedureID)
        {
            try
            {
                var videoFileData = await _services.GetVideo(procedureID);

                return File(videoFileData, "video/mp4", videoFileData.Name);
            }
            catch (Exception)
            {

                return BadRequest(JsonConvert.SerializeObject("Video for Procedure "
                    + procedureID + " Does Not Exist!"));
                throw;
            }
        }
    }
}