using dotnet_inventoryapi.DBcontext;
using dotnet_inventoryapi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using System.Xml.Linq;

namespace dotnet_inventoryapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MongoDBContext _mongoDBContext;

        public UsersController(MongoDBContext mongoDBContext)
        {
            _mongoDBContext = mongoDBContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _mongoDBContext.Users.Find(u => true).ToListAsync();
            return Ok(users);
        }

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            var user = await _mongoDBContext.Users.Find(u => u.Id == id).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(User user)
        {
            await _mongoDBContext.Users.InsertOneAsync(user);
            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdateUser(string id, User updatedUser)
        {
            var existingUser = await _mongoDBContext.Users.Find(u => u.Id == id).FirstOrDefaultAsync();

            if (existingUser == null)
            {
                return NotFound();
            }

            // Update properties as needed
            existingUser.Email = updatedUser.Email;
            existingUser.Role = updatedUser.Role;

            await _mongoDBContext.Users.ReplaceOneAsync(u => u.Id == id, existingUser);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _mongoDBContext.Users.Find(u => u.Id == id).FirstOrDefaultAsync();

            if (user == null)
            {
                return NotFound();
            }

            await _mongoDBContext.Users.DeleteOneAsync(u => u.Id == id);

            return NoContent();
        }
    }
}
