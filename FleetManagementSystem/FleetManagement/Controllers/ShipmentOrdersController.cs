using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FleetManagement.Models.Data;

namespace FleetManagement.Controllers
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
            return null;
        }

        // GET: api/ShipmentOrders/5
        [HttpGet("{id}")]
        public IActionResult GetShipmentOrder([FromRoute] int id)
        {
            return null;
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
            return null;
        }

        // DELETE: api/ShipmentOrders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteShipmentOrder([FromRoute] int id)
        {

            return null;
        }

        private bool ShipmentOrderExists(int id)
        {
            return _context.ShipmentOrder.Any(e => e.Id == id);
        }
    }
}