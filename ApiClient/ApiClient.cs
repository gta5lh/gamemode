﻿using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
using Gamemode.Models.Admin;
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

        public static async Task<long?> IDByName(string name)
        {
            HttpResponseMessage httpResponseMessage = await client.GetAsync($"http://localhost:8000/v1/users/{name}/id");

            string response = await httpResponseMessage.Content.ReadAsStringAsync();
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new System.Exception(response);
            }

            return JsonConvert.DeserializeObject<long?>(response);
        }

        public static async Task<string> MuteUser(long userId, string reason, long mutedBy, DateTime mutedAt, DateTime mutedUntil)
        {
            MuteUserRequest request = new MuteUserRequest(reason, mutedBy, mutedAt, mutedUntil);

            string json = JsonConvert.SerializeObject(request);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponseMessage = await client.PatchAsync($"http://localhost:8000/v1/users/{userId}/mute", data);

            string response = await httpResponseMessage.Content.ReadAsStringAsync();
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new System.Exception(response);
            }

            return JsonConvert.DeserializeObject<string>(response);
        }

        public static async Task<string> UnmuteUser(long userId, long unmutedBy)
        {
            UnmuteUserRequest request = new UnmuteUserRequest(unmutedBy);

            string json = JsonConvert.SerializeObject(request);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponseMessage = await client.PatchAsync($"http://localhost:8000/v1/users/{userId}/unmute", data);

            string response = await httpResponseMessage.Content.ReadAsStringAsync();
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new System.Exception(response);
            }

            return JsonConvert.DeserializeObject<string>(response);
        }

        public static async Task<string> BanUser(long userId, string reason, long bannedBy, DateTime bannedAt, DateTime bannedUntil)
        {
            BanUserRequest request = new BanUserRequest(reason, bannedBy, bannedAt, bannedUntil);

            string json = JsonConvert.SerializeObject(request);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponseMessage = await client.PatchAsync($"http://localhost:8000/v1/users/{userId}/ban", data);

            string response = await httpResponseMessage.Content.ReadAsStringAsync();
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new System.Exception(response);
            }

            return JsonConvert.DeserializeObject<string>(response);
        }

        public static async Task<string> UnbanUser(long userId, long unbannedBy)
        {
            UnbanUserRequest request = new UnbanUserRequest(unbannedBy);

            string json = JsonConvert.SerializeObject(request);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponseMessage = await client.PatchAsync($"http://localhost:8000/v1/users/{userId}/unban", data);

            string response = await httpResponseMessage.Content.ReadAsStringAsync();
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new System.Exception(response);
            }

            return JsonConvert.DeserializeObject<string>(response);
        }

        public static async Task<SetAdminRankResponse> SetAdminRank(long userId, AdminRank rank, long setBy)
        {
            SetAdminRankRequest request = new SetAdminRankRequest(rank, setBy);

            string json = JsonConvert.SerializeObject(request);
            StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

            HttpResponseMessage httpResponseMessage = await client.PatchAsync($"http://localhost:8000/v1/users/{userId}/admin-rank", data);

            string response = await httpResponseMessage.Content.ReadAsStringAsync();
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new System.Exception(response);
            }

            return JsonConvert.DeserializeObject<SetAdminRankResponse>(response);
        }
    }
}
