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
    public class ShipmentsController : ControllerBase
    {
        private readonly fleetContext _context;

        public ShipmentsController(fleetContext context)
        {
            _context = context;
        }

        // GET: api/Shipments
        [HttpGet]
        public IEnumerable<Shipment> GetShipment()
        {
            return _context.Shipment
                .Include(s=>s.Customer).ThenInclude(c => c.Address)
                .Include(s => s.Broker).ThenInclude(c => c.Address)
                .Include(s => s.Origin).ThenInclude(c => c.Address)
                .Include(s => s.Destination).ThenInclude(c => c.Address);
        }

        // GET: api/Shipments/5
        [HttpGet("{id}")]
        public IActionResult GetShipment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var shipment = _context.Shipment
                .Include(s => s.Customer).ThenInclude(c => c.Address)
                .Include(s => s.Broker).ThenInclude(c => c.Address)
                .Include(s => s.Origin).ThenInclude(c => c.Address)
                .Include(s => s.Destination).ThenInclude(c => c.Address)
                .Where(c => c.Id == id).FirstOrDefault();
            //var shipment = await _context.Shipment.FindAsync(id);

            if (shipment == null)
            {
                return NotFound();
            }

            return Ok(shipment);
        }

        // PUT: api/Shipments/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShipment([FromRoute] int id, [FromBody] Shipment shipment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != shipment.Id)
            {
                return BadRequest();
            }

            _context.Entry(shipment).State = EntityState.Modified;

            try
            {
                _context.Update(shipment);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipmentExists(id))
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

        // POST: api/Shipments
        [HttpPost]
        public async Task<IActionResult> PostShipment([FromBody] Shipment shipment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Shipment.Add(shipment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShipment",shipment);
        }

        // DELETE: api/Shipments/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipment([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shipment = await _context.Shipment.FindAsync(id);
            if (shipment == null)
            {
                return NotFound();
            }

            _context.Shipment.Remove(shipment);
            await _context.SaveChangesAsync();

            return Ok(shipment);
        }

        private bool ShipmentExists(int id)
        {
            return _context.Shipment.Any(e => e.Id == id);
        }
    }
}