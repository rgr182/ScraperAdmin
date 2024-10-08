using Microsoft.AspNetCore.Mvc;
using ScraperAdmin.DataAccess.Models;
using ScraperAdmin.DataAccess.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ScraperAdmin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateUser([FromBody] Users user)
        {
            if (user == null)
            {
                return BadRequest("User data is required.");
            }

            // Create the user asynchronously
            await _userService.AddUserAsync(user);

            return Ok(user);
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost("validateToken")]
        public async Task<ActionResult> ValidateToken([FromBody] TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest("Token is required.");
            }

            // Validate the token asynchronously
            var isValid = await _userService.ValidateTokenAsync(request.Token);
            if (isValid)
            {
                return Ok("Valid token.");
            }

            return Unauthorized("Invalid token.");
        }
    }

    public class TokenRequest
    {
        public string Token { get; set; }
    }
}
