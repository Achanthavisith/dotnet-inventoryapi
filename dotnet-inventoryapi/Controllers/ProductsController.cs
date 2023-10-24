using dotnet_inventoryapi.DBcontext;
using dotnet_inventoryapi.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace dotnet_inventoryapi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly MongoDBContext _mongoDBContext;

        public ProductsController(MongoDBContext mongoDBContext)
        {
            _mongoDBContext = mongoDBContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            var products = await _mongoDBContext.Products.Find(product => true).ToListAsync();
            return Ok(products);
        }

        [HttpGet("{id:length(24)}", Name = "GetProduct")]
        public ActionResult<Product> GetProduct(string id)
        {
            var product = _mongoDBContext.Products.Find(p => p.Id == id).FirstOrDefault();

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> CreateProduct(Product product)
        {
            await _mongoDBContext.Products.InsertOneAsync(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult UpdateProduct(string id, Product updatedProduct)
        {
            var existingProduct = _mongoDBContext.Products.FindOneAndUpdate(
                Builders<Product>.Filter.Eq(p => p.Id, id),
                Builders<Product>.Update
                    .Set(p => p.Name, updatedProduct.Name)
                    .Set(p => p.Category, updatedProduct.Category)
                    .Set(p => p.Quantity, updatedProduct.Quantity)
                    .Set(p => p.Usage, updatedProduct.Usage),
                new FindOneAndUpdateOptions<Product> { ReturnDocument = ReturnDocument.After }
            );

            if (existingProduct == null)
            {
                return NotFound();
            }

            return Ok(existingProduct);
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult DeleteProduct(string id)
        {
            var result = _mongoDBContext.Products.DeleteOne(p => p.Id == id);

            if (result.DeletedCount == 0)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
