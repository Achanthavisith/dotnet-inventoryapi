using dotnet_inventoryapi.DBcontext;
using dotnet_inventoryapi.Models;
using dotnet_inventoryapi.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

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

        [HttpGet,Authorize]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            var users = _mongoDBContext.Users.Find(u => true).ToList();
            return Ok(users);
        }

        [HttpGet("{id:length(24)}", Name = "GetUser")]
        public ActionResult<User> GetUserById(string id)
        {
            var user = _mongoDBContext.Users.Find(u => u.Id == id).First();

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        public ActionResult<User> CreateUser(User user)
        {
            var existingUser =  _mongoDBContext.Users.Find(u => u.Email == user.Email).FirstOrDefault();

            if (existingUser != null)
            {
                return Problem("User with that email already exists.");
            }

            user.Password = PasswordHasher.HashPassword(user.Password);

            _mongoDBContext.Users.InsertOne(user);
            return CreatedAtRoute("GetUser", new { id = user.Id }, user);
        }

        [HttpPut("{id:length(24)}"), Authorize]
        public ActionResult<User> UpdateUser(string id, User updatedUser)
        {
            var existingUser = _mongoDBContext.Users.Find(u => u.Id == id).FirstOrDefault();

            if (existingUser == null)
            {
                return NotFound();
            }

            // Update properties as needed
            existingUser.Email = updatedUser.Email;
            existingUser.Role = updatedUser.Role;

            _mongoDBContext.Users.ReplaceOne(u => u.Id == id, existingUser);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}"),Authorize]
        public ActionResult<User> DeleteUser(string id)
        {
            var user = _mongoDBContext.Users.Find(u => u.Id == id).FirstOrDefault();

            if (user == null)
            {
                return NotFound();
            }

            _mongoDBContext.Users.DeleteOneAsync(u => u.Id == id);

            return NoContent();
        }
    }
}
