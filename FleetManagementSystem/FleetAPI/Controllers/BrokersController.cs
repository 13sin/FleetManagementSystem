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
    public class BrokersController : ControllerBase
    {
        private readonly fleetContext _context;

        public BrokersController(fleetContext context)
        { 
            _context = context;
        }

        // GET: api/Brokers
        [HttpGet]
        public IEnumerable<Broker> GetBroker()
        {
            return _context.Broker.Include(c => c.Address);
        }

        // GET: api/Brokers/5
        [HttpGet("{id}")]
        public IActionResult GetBroker([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var broker = _context.Broker.Include(c => c.Address).Where(c => c.Id == id).FirstOrDefault();

            //var broker = await _context.Broker.FindAsync(id);

            if (broker == null)
            {
                return NotFound();
            }

            return Ok(broker);
        }

        // PUT: api/Brokers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBroker([FromRoute] int id, [FromBody] Broker broker)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != broker.Id)
            {
                return BadRequest();
            }

            _context.Entry(broker).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BrokerExists(id))
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

        // POST: api/Brokers
        [HttpPost]
        public async Task<IActionResult> PostBroker([FromBody] Broker broker)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Broker.Add(broker);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBroker", new { id = broker.Id }, broker);
        }

        // DELETE: api/Brokers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBroker([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var broker = await _context.Broker.FindAsync(id);
            if (broker == null)
            {
                return NotFound();
            }

            _context.Broker.Remove(broker);
            await _context.SaveChangesAsync();

            return Ok(broker);
        }

        private bool BrokerExists(int id)
        {
            return _context.Broker.Any(e => e.Id == id);
        }
    }
}