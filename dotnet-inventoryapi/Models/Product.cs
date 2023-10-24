using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;

namespace dotnet_inventoryapi.Models
{
    [BsonIgnoreExtraElements]
    public class Product
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [SwaggerSchema(ReadOnly = true)]
        [BsonElement("id")]
        public string? Id { get; set; }
        
        [BsonElement("name")]
        public string Name { get; set; }
        
        [BsonElement("category")]
        public string Category { get; set; }
        
        [BsonElement("quantity")]
        public int Quantity { get; set; }
        
        [BsonElement("Usage")]
        public int Usage { get; set; }

        [BsonIgnore]
        [JsonIgnore]
        public int Version { get; set; }
    }
}
