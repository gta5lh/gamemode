using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
using Newtonsoft.Json;

namespace Gamemode.ApiClient
{
    public static class ApiClient
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<User> RegisterUser(string email, string name, string password)
        {
            RegisterUserRequest request = new RegisterUserRequest(email, name, password);

            string json = JsonConvert.SerializeObject(request);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponseMessage = await client.PostAsync("http://localhost:8000/v1/users/register", data);

            string response = await httpResponseMessage.Content.ReadAsStringAsync();
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new System.Exception(response);
            }

            return JsonConvert.DeserializeObject<User>(response);
        }

        public static async Task<User> LoginUser(string email, string password)
        {
            LoginUserRequest request = new LoginUserRequest(email, password);

            string json = JsonConvert.SerializeObject(request);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponseMessage = await client.PostAsync("http://localhost:8000/v1/users/login", data);

            string response = await httpResponseMessage.Content.ReadAsStringAsync();
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new System.Exception(response);
            }

            return JsonConvert.DeserializeObject<User>(response);
        }
    }
}
