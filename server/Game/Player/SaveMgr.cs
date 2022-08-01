// <copyright file="SaveMgr.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Player
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Timers;
	using Gamemode.Game.Player.Models;
	using GamemodeCommon.Models.Data;
	using GTANetworkAPI;
	using Rpc.Player;

	public class SaveMgr : Script
	{
		private const double SavePlayerInterval120Seconds = 120000;

		private static readonly NLog.Logger Logger = Gamemode.Logger.Logger.LogFactory.GetLogger("Player.Save");

		public static void InitSavePlayerTimer()
		{
			Timer savePlayersTimer = new(SavePlayerInterval120Seconds);
			savePlayersTimer.Elapsed += OnSavePlayers;
			savePlayersTimer.AutoReset = true;
			savePlayersTimer.Start();
		}

		public static async Task SavePlayers(List<CPlayer> players)
		{
			List<SaveRequest> savePlayerRequests = new();

			NAPI.Task.Run(() =>
			{
				foreach (CPlayer player in players)
				{
					savePlayerRequests.Add(new SaveRequest(player.PKId, player.CurrentExperience, player.Money, player.GetAllWeapons(), player.Health, player.Armor));
				}
			});

			await NAPI.Task.WaitForMainThread();

			try
			{
				await Infrastructure.RpcClients.PlayerService.SaveAllAsync(new SaveAllRequest(savePlayerRequests));
			}
			catch (System.Exception ex)
			{
				Logger.Error(ex.Message);
			}
		}

		public static async Task SavePlayersOnServerStop()
		{
			List<CPlayer>? players = NAPI.Pools.GetAllPlayers().Cast<CPlayer>().Where(p => p.LoggedInAt != null).ToList();
			if (players == null || players.Count == 0)
			{
				Logger.Info("skipping players save on server stop: no players online");
				return;
			}

			List<SaveRequest> savePlayerRequests = new();

			foreach (CPlayer player in players)
			{
				savePlayerRequests.Add(new SaveRequest(player.PKId, player.CurrentExperience, player.Money, player.GetAllWeapons(), player.Health, player.Armor));
			}

			try
			{
				await Infrastructure.RpcClients.PlayerService.SaveAllAsync(new SaveAllRequest(savePlayerRequests));
			}
			catch (System.Exception ex)
			{
				Logger.Error(ex.Message);
			}
		}

		public static void Freeze(CPlayer targetPlayer, bool isFreezed)
		{
			targetPlayer.SetSharedData(DataKey.IsFreezed, isFreezed);
		}

		public static List<CPlayer> AllLoggedInPlayers()
		{
			List<CPlayer> players = new();
			foreach (CPlayer player in NAPI.Pools.GetAllPlayers().OfType<CPlayer>())
			{
				if (player.LoggedInAt == null)
				{
					continue;
				}

				players.Add(player);
			}

			return players;
		}

		private static async void OnSavePlayers(object source, ElapsedEventArgs e)
		{
			List<CPlayer>? players = null;

			NAPI.Task.Run(() => players = NAPI.Pools.GetAllPlayers().Cast<CPlayer>().Where(p => p.LoggedInAt != null).ToList());

			await NAPI.Task.WaitForMainThread();

			if (players == null || players.Count == 0)
			{
				Logger.Debug("skipping players save: no players online");
				return;
			}

			await SavePlayers(players);
		}
	}
}
