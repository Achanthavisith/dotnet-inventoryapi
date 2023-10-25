using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace dotnet_inventoryapi.Models
{
    public class Categories
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [SwaggerSchema(ReadOnly = true)]
        [BsonElement("id")]
        public string? Id { get; set; }

        [BsonElement("category")]
        public string Category { get; set; }

        [BsonElement("__v")]
        public int Version { get; set; }
    }
}
