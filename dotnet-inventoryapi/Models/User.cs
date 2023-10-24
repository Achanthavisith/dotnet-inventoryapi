using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace dotnet_inventoryapi.Models
{
    public enum UserRole
    {
        admin,
        manager,
        user
    }
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [SwaggerSchema(ReadOnly = true)]
        public string? Id { get; set; }

        [BsonElement("email")]
        public string Email { get; set; }

        [BsonElement("password")]
        [SwaggerSchema(WriteOnly = true)]
        public string Password { private get; set; }

        [BsonElement("role")]
        public UserRole Role { get; set; }

        [BsonElement("__v")]
        [JsonIgnore]
        public int Version { get; set; }
    }
}
