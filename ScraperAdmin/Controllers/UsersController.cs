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
                return BadRequest("Los datos del usuario son requeridos.");
            }

            // Crear el usuario de forma asincrónica
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
                return BadRequest("Token es requerido.");
            }

            // Validar el token de forma asincrónica
            var isValid = await _userService.ValidateTokenAsync(request.Token);
            if (isValid)
            {
                return Ok("Token válido.");
            }

            return Unauthorized("Token inválido.");
        }
    }

    public class TokenRequest
    {
        public string Token { get; set; }
    }
}
