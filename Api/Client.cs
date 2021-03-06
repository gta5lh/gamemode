namespace Gamemode.Api
{
    using System;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Net.Http.Json;
    using System.Threading.Tasks;
    using Gamemode.Models.Authentication;
    using Gamemode.Models.User;

    public static class Client
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public static void InitClient()
        {
            HttpClient.BaseAddress = new Uri("http://localhost:5000/");
            HttpClient.DefaultRequestHeaders.Accept.Clear();
            HttpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public static async Task<User> GetUserByEmailAsync(string email)
        {
            User user = null;

            HttpResponseMessage response = await HttpClient.GetAsync($"api/users/{email}");
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadFromJsonAsync<User>();
            }

            return user;
        }

        public static async Task<User> GetUserByEmailAndPasswordAsync(LoginRequest loginRequest)
        {
            User user = null;

            HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/users/login", loginRequest);
            if (response.IsSuccessStatusCode)
            {
                user = await response.Content.ReadFromJsonAsync<User>();
            }

            return user;
        }

        public static async Task<User> CreateUser(User user)
        {
            HttpResponseMessage response = await HttpClient.PostAsJsonAsync("api/users", user);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<User>();
    }
    }
}
