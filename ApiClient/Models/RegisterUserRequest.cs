using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Gamemode.ApiClient.Models
{
    public class RegisterUserRequest
    {
        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        public RegisterUserRequest(string email, string name, string password)
        {
            this.Email = email;
            this.Name = name;
            this.Password = password;
        }
    }
}
