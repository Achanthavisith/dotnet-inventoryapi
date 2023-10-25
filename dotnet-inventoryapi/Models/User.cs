using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
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
        [BsonElement("id")]
        [SwaggerSchema(ReadOnly = true)]
        public string? Id { get; set; }

        [BsonElement("email")]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [BsonElement("password")]
        public string Password { get; set; }

        [BsonElement("role")]
        public UserRole Role { get; set; }

        [BsonElement("__v")]
        [JsonIgnore]
        public int Version { get; set; }
    }
}
