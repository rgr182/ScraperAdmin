using Microsoft.AspNetCore.Mvc;
using ScraperAdmin.DataAccess.Models;
using ScraperAdmin.DataAccess.Services;

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

        // GET: api/users
        [HttpGet]
        public ActionResult<IEnumerable<Users>> GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpPost("validateToken")]
        public ActionResult ValidateToken([FromBody] TokenRequest request)
        {
            if (string.IsNullOrEmpty(request.Token))
            {
                return BadRequest("Token es requerido.");
            }

            // Aquí validamos el token
            var isValid = _userService.ValidateToken(request.Token);
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
