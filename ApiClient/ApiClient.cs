﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
using Gamemode.Models.Admin;
using GTANetworkAPI;
using Newtonsoft.Json;
using Polly;
using Polly.Retry;

namespace Gamemode.ApiClient
{
	public static class ApiClient
	{
		private static readonly HttpClient client = new HttpClient();

		static ApiClient()
		{
			string? apiURL = System.Environment.GetEnvironmentVariable("API_URL");
			if (apiURL == null)
			{
				apiURL = "http://localhost:8000/v1/";
			}

			UriBuilder uriBuilder = new UriBuilder(apiURL);
			client.BaseAddress = uriBuilder.Uri;
		}

		public static async Task<User> RegisterUser(RegisterUserRequest request)
		{
			string json = JsonConvert.SerializeObject(request);
			StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

			HttpResponseMessage httpResponseMessage = await client.PostAsync("users/register", data);

			string response = await httpResponseMessage.Content.ReadAsStringAsync();
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				throw new System.Exception(response);
			}

			return JsonConvert.DeserializeObject<User>(response);
		}

		public static async Task<User> LoginUser(LoginUserRequest request)
		{
			string json = JsonConvert.SerializeObject(request);
			StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

			HttpResponseMessage httpResponseMessage = await client.PostAsync("users/login", data);

			string response = await httpResponseMessage.Content.ReadAsStringAsync();
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				throw new System.Exception(response);
			}

			return JsonConvert.DeserializeObject<User>(response);
		}

		public static async Task<long?> IDByName(string name)
		{
			HttpResponseMessage httpResponseMessage = await client.GetAsync($"users/{name}/id");

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

			HttpResponseMessage httpResponseMessage = await client.PatchAsync($"users/{userId}/mute", data);

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

			HttpResponseMessage httpResponseMessage = await client.PatchAsync($"users/{userId}/unmute", data);

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

			HttpResponseMessage httpResponseMessage = await client.PatchAsync($"users/{userId}/ban", data);

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

			HttpResponseMessage httpResponseMessage = await client.PatchAsync($"users/{userId}/unban", data);

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

			HttpResponseMessage httpResponseMessage = await client.PatchAsync($"users/{userId}/admin-rank", data);

			string response = await httpResponseMessage.Content.ReadAsStringAsync();
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				throw new System.Exception(response);
			}

			return JsonConvert.DeserializeObject<SetAdminRankResponse>(response);
		}

		public static async Task<SetFractionResponse> SetFraction(long userId, short fraction, short tier, long setBy)
		{
			SetFractionRequest request = new SetFractionRequest(fraction, tier, setBy);

			string json = JsonConvert.SerializeObject(request);
			StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

			HttpResponseMessage httpResponseMessage = await client.PatchAsync($"users/{userId}/fraction", data);

			string response = await httpResponseMessage.Content.ReadAsStringAsync();
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				throw new System.Exception(response);
			}

			return JsonConvert.DeserializeObject<SetFractionResponse>(response);
		}

		public static async Task SaveUser(long userId, short experience, ICollection<Weapon>? weapons, long money)
		{
			SaveUserRequest request = new SaveUserRequest(experience, weapons, money);

			string json = JsonConvert.SerializeObject(request);
			StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

			HttpResponseMessage httpResponseMessage = await client.PatchAsync($"users/{userId}/save", data);

			string response = await httpResponseMessage.Content.ReadAsStringAsync();
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				throw new System.Exception(response);
			}
		}

		public static async Task<string> GiveWeapon(long userId, WeaponHash weaponHash, int amount, long givenBy)
		{
			GiveWeaponRequest request = new GiveWeaponRequest(weaponHash, amount, givenBy);

			string json = JsonConvert.SerializeObject(request);
			StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

			HttpResponseMessage httpResponseMessage = await client.PatchAsync($"users/{userId}/give-weapon", data);

			string response = await httpResponseMessage.Content.ReadAsStringAsync();
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				throw new System.Exception(response);
			}

			return JsonConvert.DeserializeObject<string>(response);
		}

		public static async Task<string> RemoveWeapon(long userId, WeaponHash weaponHash, long removedBy)
		{
			RemoveWeaponRequest request = new RemoveWeaponRequest(weaponHash, removedBy);

			string json = JsonConvert.SerializeObject(request);
			StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

			HttpResponseMessage httpResponseMessage = await client.PatchAsync($"users/{userId}/remove-weapon", data);

			string response = await httpResponseMessage.Content.ReadAsStringAsync();
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				throw new System.Exception(response);
			}

			return JsonConvert.DeserializeObject<string>(response);
		}

		public static async Task<List<Zone>> AllZones()
		{
			HttpResponseMessage httpResponseMessage = await client.GetAsync("zones");

			string response = await httpResponseMessage.Content.ReadAsStringAsync();
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				throw new System.Exception(response);
			}

			return JsonConvert.DeserializeObject<List<Zone>>(response);
		}

		public static async Task SaveUsers(SaveUsersRequest request)
		{
			string json = JsonConvert.SerializeObject(request);
			StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

			HttpResponseMessage httpResponseMessage = await client.PatchAsync($"users/save", data);

			string response = await httpResponseMessage.Content.ReadAsStringAsync();
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				throw new System.Exception(response);
			}
		}

		public static async Task<Report> CreateReport(Report request)
		{
			string json = JsonConvert.SerializeObject(request);
			StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

			HttpResponseMessage httpResponseMessage = await client.PostAsync($"reports", data);

			string response = await httpResponseMessage.Content.ReadAsStringAsync();
			if (!httpResponseMessage.IsSuccessStatusCode)
			{
				throw new System.Exception(response);
			}

			return JsonConvert.DeserializeObject<Report>(response);
		}

		public static async Task<GangWar> StartGangWar()
		{
			var retryPolicy = Policy
				.Handle<HttpRequestException>()
				.WaitAndRetryAsync(3, retryAttempt =>
					TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
					onRetry: (exception, retryCount, context) =>
					{
						// This policy might be re-used in several parts of the codebase, 
						// so we allow the logged message to be tailored.
						Logger.Logger.BaseLogger.Warn($"Retry {retryCount} of StartGangWar, due to {exception.Message}.");
					}
				);

			HttpResponseMessage httpResponseMessage = await retryPolicy.ExecuteAsync(async () =>
			{
				HttpResponseMessage httpResponseMessage = await client.PostAsync($"gang-war/start", null);
				httpResponseMessage.EnsureSuccessStatusCode();
				return httpResponseMessage;
			});

			string response = await httpResponseMessage.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<GangWar>(response);
		}

		public static async Task<GangWar> FinishGangWar(FinishGangWarRequest request)
		{
			string json = JsonConvert.SerializeObject(request);
			StringContent data = new StringContent(json, Encoding.UTF8, "application/json");

			var retryPolicy = Policy
				.Handle<HttpRequestException>()
				.WaitAndRetryAsync(3, retryAttempt =>
					TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
					onRetry: (exception, retryCount, context) =>
					{
						// This policy might be re-used in several parts of the codebase, 
						// so we allow the logged message to be tailored.
						Logger.Logger.BaseLogger.Warn($"Retry {retryCount} of FinishGangWar, due to {exception.Message}.");
					}
				);

			HttpResponseMessage httpResponseMessage = await retryPolicy.ExecuteAsync(async () =>
			{
				HttpResponseMessage httpResponseMessage = await client.PostAsync($"gang-war/finish", data);
				httpResponseMessage.EnsureSuccessStatusCode();
				return httpResponseMessage;
			});

			string response = await httpResponseMessage.Content.ReadAsStringAsync();
			return JsonConvert.DeserializeObject<GangWar>(response);
		}
	}
}
