using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class LoginUserRequest
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        public LoginUserRequest(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }
    }
}
