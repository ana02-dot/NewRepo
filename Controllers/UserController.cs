using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static readonly List<User> Users = new List<User>
        {
            new User { Id = 1, Name = "Alice", Email = "alice@example.com" },
            new User { Id = 2, Name = "Bob", Email = "bob@example.com" }
        };

        private readonly ILogger<UserController> _logger;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
        }

        // GET: api/user
        [HttpGet]
        public IActionResult GetUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                if (page <= 0 || pageSize <= 0)
                {
                    _logger.LogWarning("Invalid pagination parameters.");
                    return BadRequest("Page and pageSize must be greater than 0.");
                }

                var paginatedUsers = Users
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(paginatedUsers);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving users.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found.");
                    return NotFound();
                }

                return Ok(user);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while retrieving user with ID {id}.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // POST: api/user
        [HttpPost]
        public IActionResult CreateUser([FromBody] User newUser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid user data provided.");
                    return BadRequest(ModelState);
                }

                newUser.Id = Users.Any() ? Users.Max(u => u.Id) + 1 : 1;
                Users.Add(newUser);

                _logger.LogInformation($"User {newUser.Name} created with ID {newUser.Id}.");
                return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new user.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid user data provided.");
                    return BadRequest(ModelState);
                }

                var user = Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found.");
                    return NotFound();
                }

                user.Name = updatedUser.Name;
                user.Email = updatedUser.Email;

                _logger.LogInformation($"User with ID {id} updated.");
                return NoContent();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating user with ID {id}.");
                return StatusCode(500, "Internal server error.");
            }
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var user = Users.FirstOrDefault(u => u.Id == id);
                if (user == null)
                {
                    _logger.LogWarning($"User with ID {id} not found.");
                    return NotFound();
                }

                Users.Remove(user);

                _logger.LogInformation($"User with ID {id} deleted.");
                return NoContent();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting user with ID {id}.");
                return StatusCode(500, "Internal server error.");
            }
        }
    }

    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
    }
}