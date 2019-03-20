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
    public class ShipmentOrdersController : ControllerBase
    {
        private readonly fleetContext _context;

        public ShipmentOrdersController(fleetContext context)
        {
            _context = context;
        }

        // GET: api/ShipmentOrders
        [HttpGet]
        public IEnumerable<ShipmentOrder> GetShipmentOrder()
        {
            return _context.ShipmentOrder
                .Include(c => c.Shipment)
                .ThenInclude(s => s.Customer).ThenInclude(c => c.Address).ThenInclude(s => s.Broker).ThenInclude(c => c.Address).ThenInclude(s => s.Origin).ThenInclude(c => c.Address).ThenInclude(s => s.Destination)
                .Include(c => c.Carrier).ThenInclude(c => c.Address)
                .Include(c => c.Truck).ThenInclude(c => c.Carrier).ThenInclude(c => c.Address)
                .Include(c => c.Trailer).ThenInclude(c => c.Carrier).ThenInclude(c => c.Address)
                .Include(c => c.Driver).ThenInclude(c => c.Address).ThenInclude(c => c.Carrier).ThenInclude(c => c.Address);
        }

        // GET: api/ShipmentOrders/5
        [HttpGet("{id}")]
        public IActionResult GetShipmentOrder([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var shipmentOrder = _context.ShipmentOrder
                .Include(c => c.Shipment)
                .ThenInclude(s => s.Customer).ThenInclude(c => c.Address).ThenInclude(s => s.Broker).ThenInclude(c => c.Address).ThenInclude(s => s.Origin).ThenInclude(c => c.Address).ThenInclude(s => s.Destination)
                .Include(c => c.Carrier).ThenInclude(c => c.Address)
                .Include(c => c.Truck).ThenInclude(c => c.Carrier).ThenInclude(c => c.Address)
                .Include(c => c.Trailer).ThenInclude(c => c.Carrier).ThenInclude(c => c.Address)
                .Include(c => c.Driver).ThenInclude(c => c.Address).ThenInclude(c => c.Carrier).ThenInclude(c => c.Address).Where(c => c.Id == id);
            // var shipmentOrder = await _context.ShipmentOrder.FindAsync(id);

            if (shipmentOrder == null)
            {
                return NotFound();
            }

            return Ok(shipmentOrder);
        }

        // PUT: api/ShipmentOrders/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShipmentOrder([FromRoute] int id, [FromBody] ShipmentOrder shipmentOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != shipmentOrder.Id)
            {
                return BadRequest();
            }

            _context.Entry(shipmentOrder).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShipmentOrderExists(id))
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

        // POST: api/ShipmentOrders
        [HttpPost]
        public async Task<IActionResult> PostShipmentOrder([FromBody] ShipmentOrder shipmentOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ShipmentOrder.Add(shipmentOrder);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShipmentOrder", new { id = shipmentOrder.Id }, shipmentOrder);
        }

        // DELETE: api/ShipmentOrders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipmentOrder([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var shipmentOrder = await _context.ShipmentOrder.FindAsync(id);
            if (shipmentOrder == null)
            {
                return NotFound();
            }

            _context.ShipmentOrder.Remove(shipmentOrder);
            await _context.SaveChangesAsync();

            return Ok(shipmentOrder);
        }

        private bool ShipmentOrderExists(int id)
        {
            return _context.ShipmentOrder.Any(e => e.Id == id);
        }
    }
}