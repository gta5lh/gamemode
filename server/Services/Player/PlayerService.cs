using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gamemode.ApiClient;
using Gamemode.ApiClient.Models;
using Gamemode.Models.Player;
using GamemodeCommon.Models.Data;
using GTANetworkAPI;
using Rpc.User;

namespace Gamemode.Services.Player
{
	public static class PlayerService
	{
		public static async Task SavePlayers(List<CustomPlayer> players)
		{
			List<SaveRequest> saveUserRequests = new List<SaveRequest>();

			NAPI.Task.Run(() =>
			{
				foreach (CustomPlayer player in players)
				{
					saveUserRequests.Add(new SaveRequest(player.StaticId, player.CurrentExperience, player.Money, player.GetAllWeapons(), player.Health, player.Armor));
				}
			});

			await NAPI.Task.WaitForMainThread();

			try
			{
				await Infrastructure.RpcClients.UserService.SaveAllAsync(new SaveAllRequest(saveUserRequests));
			}
			catch (Exception ex)
			{
				Logger.Logger.BaseLogger.Error(ex.Message);
			}
		}

		public static async Task SavePlayersOnServerStop()
		{
			List<CustomPlayer> players = null;

			players = NAPI.Pools.GetAllPlayers().Cast<CustomPlayer>().Where(p => p.LoggedInAt != null).ToList();
			if (players == null || players.Count == 0)
			{
				Logger.Logger.BaseLogger.Info("skipping users save on server stop: no users online");
				return;
			}

			List<SaveRequest> saveUserRequests = new List<SaveRequest>();

			foreach (CustomPlayer player in players)
			{
				saveUserRequests.Add(new SaveRequest(player.StaticId, player.CurrentExperience, player.Money, player.GetAllWeapons(), player.Health, player.Armor));
			}

			try
			{
				await Infrastructure.RpcClients.UserService.SaveAllAsync(new SaveAllRequest(saveUserRequests));
			}
			catch (Exception ex)
			{
				Logger.Logger.BaseLogger.Error(ex.Message);
			}
		}

		public static void Freeze(CustomPlayer targetPlayer, bool isFreezed)
		{
			targetPlayer.SetSharedData(DataKey.IsFreezed, isFreezed);
		}

		public static List<CustomPlayer> AllLoggedInPlayers()
		{
			List<CustomPlayer> players = new List<CustomPlayer>();
			foreach (CustomPlayer player in NAPI.Pools.GetAllPlayers())
			{
				if (player.LoggedInAt == null) continue;
				players.Add(player);
			}

			return players;
		}
	}
}


