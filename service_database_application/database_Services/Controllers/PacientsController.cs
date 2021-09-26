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
    public class PacientsController : ControllerBase
    {
        private readonly DatabaseServicesContext _context;

        public PacientsController(DatabaseServicesContext context)
        {
            _context = context;
        }

        // GET: api/Pacients
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Pacient>>> GetPacient()
        {
            return await _context.Pacient.ToListAsync();
        }

        // GET: api/Pacients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Pacient>> GetPacient(int id)
        {
            var pacient = await _context.Pacient.FindAsync(id);

            if (pacient == null)
            {
                return NotFound();
            }

            return pacient;
        }

        // PUT: api/Pacients/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPacient(int id, Pacient pacient)
        {
            if (id != pacient.PacientID)
            {
                return BadRequest();
            }

            _context.Entry(pacient).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PacientExists(id))
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

        // POST: api/Pacients
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Pacient>> PostPacient(Pacient pacient)
        {
            _context.Pacient.Add(pacient);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPacient", new { id = pacient.PacientID }, pacient);
        }

        // DELETE: api/Pacients/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePacient(int id)
        {
            var pacient = await _context.Pacient.FindAsync(id);
            if (pacient == null)
            {
                return NotFound();
            }

            _context.Pacient.Remove(pacient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PacientExists(int id)
        {
            return _context.Pacient.Any(e => e.PacientID == id);
        }
    }
}
