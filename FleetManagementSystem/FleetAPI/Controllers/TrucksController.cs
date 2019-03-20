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
    public class TrucksController : ControllerBase
    {
        private readonly fleetContext _context;

        public TrucksController(fleetContext context)
        {
            _context = context;
        }

        // GET: api/Trucks
        [HttpGet]
        public IEnumerable<Truck> GetTruck()
        {
            return _context.Truck.Include(c => c.Carrier).ThenInclude(c => c.Address);
        }

        // GET: api/Trucks/5
        [HttpGet("{id}")]
        public IActionResult GetTruck([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var truck = _context.Truck.Include(c => c.Carrier).ThenInclude(c => c.Address).Where(c => c.Id == id);

            if (truck == null)
            {
                return NotFound();
            }

            return Ok(truck);
        }

        // PUT: api/Trucks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTruck([FromRoute] int id, [FromBody] Truck truck)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != truck.Id)
            {
                return BadRequest();
            }

            _context.Entry(truck).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TruckExists(id))
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

        // POST: api/Trucks
        [HttpPost]
        public async Task<IActionResult> PostTruck([FromBody] Truck truck)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Truck.Add(truck);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTruck", new { id = truck.Id }, truck);
        }

        // DELETE: api/Trucks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTruck([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var truck = await _context.Truck.FindAsync(id);
            if (truck == null)
            {
                return NotFound();
            }

            _context.Truck.Remove(truck);
            await _context.SaveChangesAsync();

            return Ok(truck);
        }

        private bool TruckExists(int id)
        {
            return _context.Truck.Any(e => e.Id == id);
        }
    }
}