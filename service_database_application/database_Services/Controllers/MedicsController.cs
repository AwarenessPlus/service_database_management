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
    public class MedicsController : ControllerBase
    {
        private readonly DatabaseServicesContext _context;

        public MedicsController(DatabaseServicesContext context)
        {
            _context = context;
        }

        // GET: api/Medics
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medic>>> GetMedic()
        {
            return await _context.Medic.ToListAsync();
        }

        // GET: api/Medics/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Medic>> GetMedic(int id)
        {
            var medic = await _context.Medic.FindAsync(id);

            if (medic == null)
            {
                return NotFound();
            }

            return medic;
        }

        // PUT: api/Medics/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMedic(int id, Medic medic)
        {
            if (id != medic.MedicID)
            {
                return BadRequest();
            }

            _context.Entry(medic).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MedicExists(id))
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

        // POST: api/Medics
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Medic>> PostMedic(Medic medic)
        {
            _context.Medic.Add(medic);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMedic", new { id = medic.MedicID }, medic);
        }

        // DELETE: api/Medics/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedic(int id)
        {
            var medic = await _context.Medic.FindAsync(id);
            if (medic == null)
            {
                return NotFound();
            }

            _context.Medic.Remove(medic);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MedicExists(int id)
        {
            return _context.Medic.Any(e => e.MedicID == id);
        }
    }
}
