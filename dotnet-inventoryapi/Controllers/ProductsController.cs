using dotnet_inventoryapi.DBcontext;
using dotnet_inventoryapi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace dotnet_inventoryapi.Controllers
{
    [Authorize]
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
        public ActionResult<IEnumerable<Product>> GetProducts()
        {
            var products = _mongoDBContext.Products.Find(product => true).ToList();
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
        public ActionResult<Product> CreateProduct(Product product)
        {
            _mongoDBContext.Products.InsertOne(product);

            return CreatedAtRoute("GetProduct", new { id = product.Id }, product);
        }

        [HttpPut("{id:length(24)}")]
        public ActionResult<Product> UpdateProduct(string id, Product updatedProduct)
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
        public ActionResult<Product> DeleteProduct(string id)
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
