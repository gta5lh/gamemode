// <copyright file="AuthenticationController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Gamemode.ApiClient.Models;
	using Gamemode.Models.Player;
	using Gamemode.Services;
	using Grpc.Core;
	using GTANetworkAPI;
	using Newtonsoft.Json;
	using Rollbar;
	using Rpc.Errors;
	using Rpc.User;

	public class AuthenticationController : Script
	{
		[RemoteEvent("LoginSubmitted")]
		private async Task OnLoginSubmitted(CustomPlayer player, string request)
		{
			if (ResourceStartController.ShouldWait(player.Id))
			{
				NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "WaitAuthenticationAction");
				return;
			}

			GamemodeCommon.Authentication.Models.LoginRequest loginRequest = JsonConvert.DeserializeObject<GamemodeCommon.Authentication.Models.LoginRequest>(request);
			List<string> invalidFieldNames = loginRequest.Validate();
			if (invalidFieldNames.Count > 0)
			{
				NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "LoginSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
				return;
			}

			LoginResponse loginResponse;

			try
			{
				LoginRequest loginUserRequest = new LoginRequest(loginRequest.Email, loginRequest.Password, player.Address, player.SocialClubId.ToString(), player.Serial, player.GameType);
				loginResponse = await Infrastructure.RpcClients.UserService.LoginAsync(loginUserRequest);
			}
			catch (RpcException e)
			{
				invalidFieldNames = new List<string>(new string[] { "email", "password" });

				if (Error.IsEqualErrorCode(e.StatusCode, ErrorCode.UserBanned))
				{
					invalidFieldNames = new List<string>(new string[] { "banned" });
				}
				else if (Error.IsEqualErrorCode(e.StatusCode, ErrorCode.AlreadyLoggedIn))
				{
					invalidFieldNames = new List<string>(new string[] { "already_online" });
				}
				else if (!Error.IsEqualErrorCode(e.StatusCode, ErrorCode.UserNotFound))
				{
					RollbarLocator.RollbarInstance.Error(e);
				}

				NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "LoginSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
				return;
			}
			catch (Exception e)
			{
				RollbarLocator.RollbarInstance.Error(e);
				invalidFieldNames = new List<string>(new string[] { "email", "password" });
				NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "LoginSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
				return;
			}

			NAPI.Task.Run(() =>
			{
				if (PlayerUtil.GetByStaticId(loginResponse.User.ID) != null)
				{
					invalidFieldNames = new List<string>(new string[] { "already_online" });
					NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "LoginSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
					return;
				}

				CustomPlayer.LoadPlayerCache(player, loginResponse.User);
				NAPI.ClientEvent.TriggerClientEvent(player, "LogIn");
				NAPI.Player.SpawnPlayer(player, new Vector3(0, 0, 0));
				GangWarService.DisplayGangWarUI(player);
			});
		}

		[RemoteEvent("RegisterSubmitted")]
		private async Task OnRegisterSubmitted(CustomPlayer player, string request)
		{
			if (ResourceStartController.ShouldWait(player.Id))
			{
				NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "WaitAuthenticationAction");
				return;
			}

			GamemodeCommon.Authentication.Models.RegisterRequest registerRequest = JsonConvert.DeserializeObject<GamemodeCommon.Authentication.Models.RegisterRequest>(request);
			List<string> invalidFieldNames = registerRequest.Validate();
			if (invalidFieldNames.Count > 0)
			{
				NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "RegisterSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
				return;
			}

			RegisterResponse registerResponse;

			try
			{
				RegisterRequest registerUserRequest = new RegisterRequest(registerRequest.Email, registerRequest.Username, registerRequest.Password, player.Address, player.SocialClubId.ToString(), player.Serial, player.GameType);
				registerResponse = await Infrastructure.RpcClients.UserService.RegisterAsync(registerUserRequest);
			}
			catch (RpcException e)
			{
				if (Error.IsEqualErrorCode(e.StatusCode, ErrorCode.UsernameAlreadyExists))
				{
					invalidFieldNames = new List<string>(new string[] { "username_already_exists" });
				}

				if (Error.IsEqualErrorCode(e.StatusCode, ErrorCode.EmailAlreadyExists))
				{
					invalidFieldNames = new List<string>(new string[] { "email_already_exists" });
				}

				NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "RegisterSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
				return;
			}
			catch (Exception)
			{
				invalidFieldNames = new List<string>(new string[] { "internal_server_error" });
				NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "RegisterSubmittedFailed", JsonConvert.SerializeObject(invalidFieldNames));
				return;
			}

			NAPI.Task.Run(() =>
			{
				CustomPlayer.LoadPlayerCache(player, registerResponse.User);
				NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "LogIn");
				NAPI.Player.SpawnPlayer(player, new Vector3(0, 0, 0));
				GangWarService.DisplayGangWarUI(player);
			});
		}

		[ServerEvent(Event.PlayerDisconnected)]
		private async Task OnPlayerDisconnected(CustomPlayer player, DisconnectionType disconnectType, string reason)
		{
			if (player.LoggedInAt == null) return;

			CustomPlayer.UnloadPlayerCache(player);

			try
			{
				await Infrastructure.RpcClients.UserService.LogoutAsync(new LogoutRequest(player.StaticId, player.Money, player.CurrentExperience, player.GetAllWeapons()));
			}
			catch
			{
			}
		}
	}
}
