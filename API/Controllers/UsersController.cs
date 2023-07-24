using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetAllUsers()
        {
            List<AppUser> users = await _context.Users.ToListAsync();
            if (users.Count == 0)
                return Array.Empty<AppUser>();
            return Ok(users);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<AppUser>> GetUserById(int id)
        {
            try
            {
                if (id == 0)
                    return BadRequest();
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                    return NotFound($"User with id {id} not found.");
                return Ok(user);
            }
            catch (Exception)
            {

                // Handle any other unexpected exceptions here.
                // Log the error, return an appropriate response, or rethrow the exception.
                return StatusCode(500, "An error occurred while processing your request.");
            }

        }
        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser(AppUser user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                // Handle any other unexpected exceptions here.
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }

    }

}