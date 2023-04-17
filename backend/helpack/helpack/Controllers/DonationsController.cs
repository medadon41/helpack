using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using helpack.Data;
using helpack.Data.Entities;

namespace helpack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DonationsController : ControllerBase
    {
        private readonly HelpackDbContext _context;

        public DonationsController(HelpackDbContext context)
        {
            _context = context;
        }


        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<IEnumerable<Donation>>> GetDonations()
        {
          if (_context.Donations == null)
          {
              return NotFound();
          }
          return await _context.Donations.ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Donation>> GetDonation(int id)
        {
          if (_context.Donations == null)
          {
              return NotFound();
          }
          var donation = await _context.Donations.FindAsync(id);

          if (donation == null)
          { 
              return NotFound();
          }

          return donation;
        }


        [HttpPut("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> PutDonation(int id, Donation donation)
        {
            if (id != donation.Id)
            {
                return BadRequest();
            }

            _context.Entry(donation).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DonationExists(id))
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


        [HttpPost]
        public async Task<ActionResult<Donation>> PostDonation(Donation donation)
        {
          if (_context.Donations == null)
          {
              return Problem("Entity set 'HelpackDbContext.Donations'  is null.");
          }
          _context.Donations.Add(donation);
          var profile = await _context.Profiles.FirstOrDefaultAsync(i => i.Id == donation.ReceiverId);
          profile.DonationsRaised += donation.Amount;
          await _context.SaveChangesAsync();

          return CreatedAtAction("GetDonation", new { id = donation.Id }, donation);
        }

        // DELETE: api/Donations/5
        [HttpDelete("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> DeleteDonation(int id)
        {
            if (_context.Donations == null)
            {
                return NotFound();
            }
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
            {
                return NotFound();
            }

            _context.Donations.Remove(donation);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DonationExists(int id)
        {
            return (_context.Donations?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
