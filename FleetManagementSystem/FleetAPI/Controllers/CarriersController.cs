using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using fleetAPI.Models.Data;
using AutoMapper;
using fleetAPI.Models;
using AutoMapper.QueryableExtensions;

namespace fleetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarriersController : ControllerBase
    {
		
        private readonly fleetContext _context;
      
	  
        public CarriersController(fleetContext context)//, IMapper mapper)
        {
            _context = context;
            //_mapper = mapper;
        }

        // GET: api/Carriers
        [HttpGet]
        public IEnumerable<Carrier> GetCarrier()
        {


            var carriers = _context.Carrier.Include(c => c.Address);
            //return _context.Carrier;
            return carriers;
        }

        // GET: api/Carriers/5
        [HttpGet("{id}")]
        public IActionResult GetCarrier([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //var carrier = _context.Carrier.Include(c => c.Address).ToHashSet().Where(c=>c.Id==id);


            var carrier = _context.Carrier.Include(c => c.Address).Where(c => c.Id == id).FirstOrDefault();
            // var carrier = await _context.Carrier.FindAsync(id);
            if (carrier == null)
            {
                return NotFound();
            }

            return Ok(carrier);
        }

        // PUT: api/Carriers/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarrier([FromRoute] int id, [FromBody] Carrier carrier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != carrier.Id)
            {
                return BadRequest();
            }

            _context.Entry(carrier).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarrierExists(id))
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

        // POST: api/Carriers
        [HttpPost]
        public async Task<IActionResult> PostCarrier([FromBody] Carrier carrier)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _context.Carrier.Add(carrier);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCarrier", carrier);
        }

        // DELETE: api/Carriers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarrier([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var carrier = await _context.Carrier.FindAsync(id);
            if (carrier == null)
            {
                return NotFound();
            }

            _context.Carrier.Remove(carrier);
            await _context.SaveChangesAsync();

            return Ok(carrier);
        }

        private bool CarrierExists(int id)
		
        {
            return _context.Carrier.Any(e => e.Id == id);
        }
    }
}