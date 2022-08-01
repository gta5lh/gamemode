// <copyright file="PlayerConnet.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Auth.Events
{
	using System.Threading.Tasks;
	using Gamemode.Game.Player.Models;
	using Gamemode.Game.ServerSettings;
	using Grpc.Core;
	using GTANetworkAPI;
	using Rpc.Errors;
	using Rpc.Player;

	public class PlayerConnect : Script
	{
		[ServerEvent(Event.PlayerConnected)]
		private static async Task OnPlayerConnected(CPlayer player)
		{
			if (player.LoggedInAt != null)
			{
				return;
			}

			LoginResponse loginResponse;

			try
			{
				LoginRequest loginPlayerRequest = new("87964340-d699-456d-b853-ba728778326c", Settings.ServerID, player.Address, player.SocialClubId.ToString(), player.Serial, player.GameType);
				loginResponse = await Infrastructure.RpcClients.PlayerService.LoginAsync(loginPlayerRequest);
			}
			catch (RpcException e)
			{
				if (Error.IsEqualErrorCode(e.StatusCode, ErrorCode.PlayerBanned))
				{
					NAPI.Task.Run(() => player.SendChatMessage("banned"));
				}
				else if (Error.IsEqualErrorCode(e.StatusCode, ErrorCode.AlreadyLoggedIn))
				{
					NAPI.Task.Run(() => player.SendChatMessage("already_online"));
				}
				else if (!Error.IsEqualErrorCode(e.StatusCode, ErrorCode.PlayerNotFound) && !Error.IsEqualErrorCode(e.StatusCode, ErrorCode.InvalidPassword))
				{
					NAPI.Task.Run(() => player.SendChatMessage("unknown"));
				}

				return;
			}

			NAPI.Task.Run(() => CPlayer.LoadPlayerCache(player, loginResponse.Player));
		}
	}
}
