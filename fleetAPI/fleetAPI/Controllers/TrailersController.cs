using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using fleetAPI.Models.Data;

namespace fleetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrailersController : ControllerBase
    {
        private readonly fleetContext _context;

        public TrailersController(fleetContext context)
        {
            _context = context;
        }

        // GET: api/Trailers
        [HttpGet]
        public IEnumerable<Trailer> GetTrailer()
        {
            return _context.Trailer.Include(c => c.Carrier).ThenInclude(c=>c.Address);
        }

        // GET: api/Trailers/5
        [HttpGet("{id}")]
        public IActionResult GetTrailer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var trailer = _context.Trailer.Include(c => c.Carrier).ThenInclude(c => c.Address).Where(c => c.Id == id).FirstOrDefault();
            //var trailer = await _context.Trailer.FindAsync(id);

            if (trailer == null)
            {
                return NotFound();
            }

            return Ok(trailer);
        }

        // PUT: api/Trailers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrailer([FromRoute] int id, [FromBody] Trailer trailer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != trailer.Id)
            {
                return BadRequest();
            }

            _context.Entry(trailer).State = EntityState.Modified;

            try
            {
                _context.Update(trailer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrailerExists(id))
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

        // POST: api/Trailers
        [HttpPost]
        public async Task<IActionResult> PostTrailer([FromBody] Trailer trailer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Trailer.Add(trailer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTrailer", new { id = trailer.Id }, trailer);
        }

        // DELETE: api/Trailers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrailer([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var trailer = await _context.Trailer.FindAsync(id);
            if (trailer == null)
            {
                return NotFound();
            }

            _context.Trailer.Remove(trailer);
            await _context.SaveChangesAsync();

            return Ok(trailer);
        }

        private bool TrailerExists(int id)
        {
            return _context.Trailer.Any(e => e.Id == id);
        }
    }
}