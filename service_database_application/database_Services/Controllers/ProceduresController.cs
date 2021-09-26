using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Domain_Model_App;
using database_Services.Data;

namespace database_Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProceduresController : ControllerBase
    {
        private readonly DatabaseServicesContext _context;

        public ProceduresController(DatabaseServicesContext context)
        {
            _context = context;
        }

        // GET: api/Procedures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Procedure>>> GetProcedure()
        {
            return await _context.Procedure.ToListAsync();
        }

        // GET: api/Procedures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Procedure>> GetProcedure(int id)
        {
            var procedure = await _context.Procedure.FindAsync(id);

            if (procedure == null)
            {
                return NotFound();
            }

            return procedure;
        }

        // PUT: api/Procedures/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProcedure(int id, Procedure procedure)
        {
            if (id != procedure.ProcedureID)
            {
                return BadRequest();
            }

            _context.Entry(procedure).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProcedureExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Procedures
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Procedure>> PostProcedure(Procedure procedure)
        {
            _context.Procedure.Add(procedure);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProcedure", new { id = procedure.ProcedureID }, procedure);
        }

        // DELETE: api/Procedures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProcedure(int id)
        {
            var procedure = await _context.Procedure.FindAsync(id);
            if (procedure == null)
            {
                return NotFound();
            }

            _context.Procedure.Remove(procedure);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProcedureExists(int id)
        {
            return _context.Procedure.Any(e => e.ProcedureID == id);
        }
    }
}
