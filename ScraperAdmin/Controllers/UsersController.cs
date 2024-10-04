using Microsoft.AspNetCore.Mvc;
using ScraperAdmin.DataAccess.Models;
using ScraperAdmin.DataAccess.Services;
using System.Collections.Generic;

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

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public ActionResult<Users> GetUserById(int id)
        {
            var user = _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public ActionResult AddUser([FromBody] Users user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _userService.AddUser(user);
            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        }

        // PUT: api/users/{id}
        [HttpPut("{id}")]
        public ActionResult UpdateUser(int id, [FromBody] Users user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingUser = _userService.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            _userService.UpdateUser(user);
            return NoContent();
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public ActionResult DeleteUser(int id)
        {
            var existingUser = _userService.GetUserById(id);
            if (existingUser == null)
            {
                return NotFound();
            }

            _userService.DeleteUser(id);
            return NoContent();
        }
    }
}
