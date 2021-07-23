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
		[RemoteProc("LoginSubmitted", true)]
		private async Task<System.Object> OnLoginSubmitted(CustomPlayer player, string request)
		{
			List<string> invalidFieldNames = new List<string>();
			if (ResourceStartController.ShouldWait(player.Id))
			{
				invalidFieldNames.Add("wait");
				return JsonConvert.SerializeObject(invalidFieldNames);
			}

			GamemodeCommon.Authentication.Models.LoginRequest loginRequest = JsonConvert.DeserializeObject<GamemodeCommon.Authentication.Models.LoginRequest>(request);
			invalidFieldNames = loginRequest.Validate();
			if (invalidFieldNames.Count > 0)
			{
				return JsonConvert.SerializeObject(invalidFieldNames);
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
				else if (!Error.IsEqualErrorCode(e.StatusCode, ErrorCode.UserNotFound) && !Error.IsEqualErrorCode(e.StatusCode, ErrorCode.InvalidPassword))
				{
					RollbarLocator.RollbarInstance.Error(e);
				}

				return JsonConvert.SerializeObject(invalidFieldNames);
			}
			catch (Exception e)
			{
				RollbarLocator.RollbarInstance.Error(e);
				invalidFieldNames = new List<string>(new string[] { "email", "password" });
				return JsonConvert.SerializeObject(invalidFieldNames);
			}

			bool alreadyOnline = false;

			NAPI.Task.Run(() =>
			{
				if (PlayerUtil.GetByStaticId(loginResponse.User.ID) != null)
				{
					alreadyOnline = true;
					return;
				}

				CustomPlayer.LoadPlayerCache(player, loginResponse.User);
				NAPI.Player.SpawnPlayer(player, new Vector3(0, 0, 0));
				GangWarService.DisplayGangWarUI(player);
			});

			if (alreadyOnline)
			{
				invalidFieldNames = new List<string>(new string[] { "internal_server_error" });
				return JsonConvert.SerializeObject(invalidFieldNames);
			}

			return "";
		}

		[RemoteProc("RegisterSubmitted", true)]
		private async Task<System.Object> OnRegisterSubmitted(CustomPlayer player, string request)
		{
			List<string> invalidFieldNames = new List<string>();
			if (ResourceStartController.ShouldWait(player.Id))
			{
				invalidFieldNames.Add("wait");
				return JsonConvert.SerializeObject(invalidFieldNames);
			}

			GamemodeCommon.Authentication.Models.RegisterRequest registerRequest = JsonConvert.DeserializeObject<GamemodeCommon.Authentication.Models.RegisterRequest>(request);
			invalidFieldNames = registerRequest.Validate();
			if (invalidFieldNames.Count > 0)
			{
				return JsonConvert.SerializeObject(invalidFieldNames);
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

				return JsonConvert.SerializeObject(invalidFieldNames);
			}
			catch (Exception)
			{
				invalidFieldNames = new List<string>(new string[] { "internal_server_error" });
				return JsonConvert.SerializeObject(invalidFieldNames);
			}

			NAPI.Task.Run(() =>
			{
				CustomPlayer.LoadPlayerCache(player, registerResponse.User);
				NAPI.Player.SpawnPlayer(player, new Vector3(0, 0, 0));
				GangWarService.DisplayGangWarUI(player);
			});

			return "";
		}

		[RemoteProc("ResetPasswordSubmitted", true)]
		private async Task<System.Object> OnResetPasswordSubmitted(CustomPlayer player, string request)
		{
			List<string> invalidFieldNames = new List<string>();
			if (ResourceStartController.ShouldWait(player.Id))
			{
				invalidFieldNames.Add("wait");
				return JsonConvert.SerializeObject(invalidFieldNames);
			}

			GamemodeCommon.Authentication.Models.ResetPasswordRequest resetPasswordRequest = JsonConvert.DeserializeObject<GamemodeCommon.Authentication.Models.ResetPasswordRequest>(request);
			invalidFieldNames = resetPasswordRequest.Validate();
			if (invalidFieldNames.Count > 0)
			{
				return JsonConvert.SerializeObject(invalidFieldNames);
			}

			RequestResetPasswordResponse requestResetPasswordResponse;

			try
			{
				RequestResetPasswordRequest requestResetPasswordRequest = new RequestResetPasswordRequest(resetPasswordRequest.Email);
				requestResetPasswordResponse = await Infrastructure.RpcClients.UserService.RequestResetPasswordAsync(requestResetPasswordRequest);
			}
			catch (RpcException e)
			{
				if (Error.IsEqualErrorCode(e.StatusCode, ErrorCode.UserNotFound))
				{
					invalidFieldNames = new List<string>(new string[] { "user_not_found" });
				}

				return JsonConvert.SerializeObject(invalidFieldNames);
			}
			catch (Exception)
			{
				invalidFieldNames = new List<string>(new string[] { "internal_server_error" });
				return JsonConvert.SerializeObject(invalidFieldNames);
			}

			if (requestResetPasswordResponse.HasWaitSeconds)
			{
				invalidFieldNames.Add($"wait_{requestResetPasswordResponse.WaitSeconds}");
				return JsonConvert.SerializeObject(invalidFieldNames);
			}

			return "";
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
