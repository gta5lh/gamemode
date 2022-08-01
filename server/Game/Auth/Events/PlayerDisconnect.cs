// <copyright file="PlayerDisconnect.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Auth.Events
{
	using System.Threading.Tasks;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;
	using Rpc.Player;

	public class PlayerDisconnect : Script
	{
		[ServerEvent(Event.PlayerDisconnected)]
		private static async Task OnPlayerDisconnected(CPlayer player, DisconnectionType disconnectType, string reason)
		{
			if (player.LoggedInAt == null)
			{
				return;
			}

			CPlayer.UnloadPlayerCache(player);

			try
			{
				await Infrastructure.RpcClients.PlayerService.LogoutAsync(new LogoutRequest(player.PKId, player.Money, player.CurrentExperience, player.GetAllWeapons(), player.Health, player.Armor));
			}
			catch (System.Exception e)
			{
				Logger.Logger.BaseLogger.Error(e);
			}
		}
	}
}
