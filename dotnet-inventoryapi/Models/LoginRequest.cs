using System.Text.Json.Serialization;

namespace dotnet_inventoryapi.Models
{
    public class LoginRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

    }
}
