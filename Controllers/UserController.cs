using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UserManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static List<User> Users = new List<User>
    {
        new User { Id = 1, Name = "Alice", Email = "alice@example.com" },
        new User { Id = 2, Name = "Bob", Email = "bob@example.com" }
    };

        // GET: api/user
        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(Users);
        }

        // GET: api/user/{id}
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        // POST: api/user
        [HttpPost]
        public IActionResult CreateUser([FromBody] User newUser)
        {
            newUser.Id = Users.Max(u => u.Id) + 1;
            Users.Add(newUser);
            return CreatedAtAction(nameof(GetUserById), new { id = newUser.Id }, newUser);
        }

        // PUT: api/user/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            return NoContent();
        }

        // DELETE: api/user/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return NotFound();

            Users.Remove(user);
            return NoContent();
        }
    }
}
