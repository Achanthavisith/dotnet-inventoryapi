using Microsoft.AspNetCore.Mvc;
using dotnet_inventoryapi.DBcontext;
using dotnet_inventoryapi.Models;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Driver;

namespace dotnet_inventoryapi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly MongoDBContext _mongoDBContext;

        public CategoriesController(MongoDBContext mongoDBContext)
        {
            _mongoDBContext = mongoDBContext;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Categories>> Get()
        {
            var categories = _mongoDBContext.Categories.Find(_ => true).ToList();
            return Ok(categories);
        }

        [HttpGet("{id}")]
        public ActionResult<Categories> GetById(string id)
        {
            var category = _mongoDBContext.Categories.Find(c => c.Id == id).FirstOrDefault();

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public ActionResult<Categories> Post([FromBody] Categories category)
        {
             _mongoDBContext.Categories.InsertOne(category);
            return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
        }

        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] Categories updatedCategory)
        {
            var filter = Builders<Categories>.Filter.Eq(c => c.Id, id);
            var update = Builders<Categories>.Update
                .Set(c => c.Category, updatedCategory.Category)
                .Set(c => c.Version, updatedCategory.Version);

            var result = _mongoDBContext.Categories.UpdateOne(filter, update);

            if (result.ModifiedCount == 0)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(string id)
        {
            var result = _mongoDBContext.Categories.DeleteOne(c => c.Id == id);

            if (result.DeletedCount == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}

