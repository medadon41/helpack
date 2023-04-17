using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using helpack.Data;
using helpack.Data.Entities;
using helpack.DTO;
using helpack.Services;
using MimeKit;
using shortid;

namespace helpack.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly HelpackDbContext _context;
        private readonly IMailService _mailService;

        public UsersController(HelpackDbContext context, IMailService mailService)
        {
            _context = context;
            _mailService = mailService;
        }

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<ActionResult<IEnumerable<HelpackUser>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
          return await _context.Users.ToListAsync();
        }
        
        
        [HttpGet("{id}")]
        public async Task<ActionResult<HelpackUser>> GetHelpackUser(int id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
          var helpackUser = await _context.Users.FindAsync(id);

          if (helpackUser == null)
          {
              return NotFound();
          }

          return helpackUser;
        }

        [HttpPost("Check")]
        public async Task<ActionResult<HelpackUser>> GetUserByUsername(UserCheckViewModel user)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var helpackUser = await _context.Users.FirstOrDefaultAsync(n => n.UserName == user.UserName || n.Email == user.Email);

            if (helpackUser == null)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutHelpackUser(int id, HelpackUser helpackUser)
        { 
            if (id != helpackUser.Id)
            {
                return BadRequest();
            }

            helpackUser.Password = HashPassword(helpackUser.Password);
            _context.Entry(helpackUser).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HelpackUserExists(id))
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
        public async Task<ActionResult<HelpackUser>> PostHelpackUser(HelpackUser helpackUser)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'HelpackDbContext.Users'  is null.");
          }

          helpackUser.Password = HashPassword(helpackUser.Password);
          _context.Users.Add(helpackUser);
          _context.Profiles.Add(new Profile
          {
              Author = helpackUser,
              DonationsRaised = 0
          });
          await _context.SaveChangesAsync();

          return CreatedAtAction("GetHelpackUser", new { id = helpackUser.Id }, helpackUser);
        }

        
        [HttpPost("Login")]
        public async Task<ActionResult<HelpackUser>> LoginUser(UserLoginViewModel userLoginViewModel)
        {
            if (_context.Users == null)
            {
                return Problem("Entity set 'HelpackDbContext.Users'  is null.");
            }

            userLoginViewModel.Password = HashPassword(userLoginViewModel.Password);

            var user = await _context.Users.FirstOrDefaultAsync(l => l.Email == userLoginViewModel.Email);

            if (user == null)
            {
                return NotFound(new
                {
                    Error = "User not found" 
                });
            }
            if (user.Password != userLoginViewModel.Password)
            {
                return BadRequest(new
                {
                    Error = "Incorrect password"
                });
            }

            return Ok(user);
        }
        
        [HttpGet("Recover/{email}")]
        public async Task<ActionResult<HelpackUser>> SendRecoveryEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(n => n.Email == email);

            if (user == null)
            {
                return NotFound();
            }

            string newPass = ShortId.Generate();
            

            var request = new MailRequest
            {
                ToEmail = user.Email,
                UserName = user.UserName,
                Password = newPass
            };
            

            user.Password = HashPassword(newPass);
            
            try
            {
                await _mailService.SendEmailAsync(request);
                await _context.SaveChangesAsync();
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        
        
        [HttpDelete("{id}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> DeleteHelpackUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var helpackUser = await _context.Users.FindAsync(id);
            if (helpackUser == null)
            {
                return NotFound();
            }

            _context.Users.Remove(helpackUser);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HelpackUserExists(int id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();

            return hash;
        }
    }
}
