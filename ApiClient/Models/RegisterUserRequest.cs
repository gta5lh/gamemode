﻿using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class RegisterUserRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        public RegisterUserRequest(string email, string name, string password)
        {
            this.Email = email;
            this.Name = name;
            this.Password = password;
        }
    }
}
