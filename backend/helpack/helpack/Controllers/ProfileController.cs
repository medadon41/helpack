using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using helpack.Data;
using helpack.DTO;
using helpack.Misc;
using Profile = helpack.Data.Entities.Profile;

namespace helpack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly HelpackDbContext _context;
        private readonly IMapper _mapper;

        public ProfileController(HelpackDbContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProfileCardViewModel>>> GetProfiles()
        {
          if (_context.Profiles == null)
          {
              return NotFound();
          }

          var items = await _context.Profiles
              .Include(a => a.Author)
              .Where(t => t.Title != null)
              .ToListAsync();
          var viewModels = _mapper.Map<IEnumerable<ProfileCardViewModel>>(items);
          return Ok(viewModels);
        }
        
        
        [HttpGet("{id}")]
        public async Task<ActionResult<ProfileViewModel>> GetProfile(int id)
        {
          if (_context.Profiles == null)
          {
              return NotFound();
          }
          
          var profile = await _context.Profiles
              .Include(a => a.Author)
              .Include(d => d.Donations)
              .FirstOrDefaultAsync(i => i.Id == id && i.Title != null);

          if (profile == null)
          {
              return NotFound();
          }

          var donationsViewModel = _mapper.Map<IEnumerable<DonationScoreboardViewModel>>(profile.Donations);
          var profileViewModel = _mapper.Map<ProfileViewModel>(profile);
          profileViewModel.Donations = donationsViewModel;
          return Ok(profileViewModel);
        }

        [HttpGet("{id}/Insights")]
        public async Task<ActionResult<ProfileSettingsViewModel>> GetInsightsInfo(int id)
        {
            if (_context.Profiles == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles
                .Include(a => a.Author)
                .Include(d => d.Donations)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (profile == null)
            {
                return NotFound();
            }
            
            var donationsViewModel = _mapper.Map<IEnumerable<DonationViewModel>>(profile.Donations);
            var profileInsightsViewModel = _mapper.Map<ProfileInsightsViewModel>(profile);
            profileInsightsViewModel.Donations = donationsViewModel;
            return Ok(profileInsightsViewModel);
        }

        [HttpGet("{id}/Update")]
        public async Task<ActionResult<ProfileSettingsViewModel>> GetSettingsInfo(int id)
        {
            if (_context.Profiles == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles.FirstOrDefaultAsync(a => a.AuthorId == id);

            if (profile == null)
            {
                return NotFound();
            }

            var profileSettingsViewModel = _mapper.Map<ProfileSettingsViewModel>(profile);
            return Ok(profileSettingsViewModel);
        }

        [HttpGet("{id}/Donate")]
        public async Task<ActionResult<ProfileDonateViewModel>> GetDonationInfo(int id)
        {
            if (_context.Profiles == null)
            {
                return NotFound();
            }

            var profile = await _context.Profiles.FindAsync(id);

            if (profile == null)
            {
                return NotFound();
            }

            var profileSettingsViewModel = _mapper.Map<ProfileDonateViewModel>(profile);
            return Ok(profileSettingsViewModel);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProfile(int id, [FromForm] ProfileUpdateModel model)
        {
            // if (id != viewModel.Id)
            // {
            //     return BadRequest();
            // }

            var profile = _mapper.Map<Profile>(model);
            var oldProfile = await _context.Profiles.FindAsync(id);
            
            if (model.Image != null)
            {
                var cloudinary = new Cloudinary(new Account("mddn41", "746897198495964", "3HQZOiscr0DfW48TeNia-MZwvks"));

                using var stream = new MemoryStream();
                model.Image.CopyTo(stream);
                stream.Position = 0;
                
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(model.Image.FileName, stream),
                };
                var uploadResult = cloudinary.Upload(uploadParams);
                string link = $"{uploadResult.Url}";

                profile.ImageUrl = link;
            }
            else
            {
                profile.ImageUrl = oldProfile.ImageUrl;
            }
            
            profile.DonationsRaised = oldProfile.DonationsRaised;

            _context.Entry(profile).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProfileExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }
        
        
        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<Profile>> PostProfile(Profile profile)
        {
          if (_context.Profiles == null)
          {
              return Problem("Entity set 'HelpackDbContext.Profiles'  is null.");
          }
          _context.Profiles.Add(profile);
          await _context.SaveChangesAsync();

          return CreatedAtAction("GetProfile", new { id = profile.Id }, profile);
        }


        [HttpDelete("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> DeleteProfile(int id)
        { 
            if (_context.Profiles == null)
            {
                return NotFound();
            }
            var profile = await _context.Profiles.FindAsync(id);
            if (profile == null)
            {
                return NotFound();
            }

            _context.Profiles.Remove(profile);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProfileExists(int id)
        {
            return (_context.Profiles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
