using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
using Gamemode.Cache.GangWar;
using Gamemode.Cache.GangZone;
using Gamemode.Colshape;
using Gamemode.Models.Player;
using GTANetworkAPI;
using Rpc.GangWar;

namespace Gamemode.Services
{
	public static class GangWarService
	{
		const string initGangWarChatMessage = "Начинается война за территорию через 10 минут! Нападение на {0}";

		const string startGangWarChatMessage = "Началась война за территорию! Нападение на {0}";

		const string finishGangWarChatMessage1 = "Закончилась война за территорию! ";
		const string finishGangWarChatMessage2 = "Победили {0}";
		const string finishGangWarChatMessage3 = "Территория осталась во владении {0}";
		const string finishGangWarBecauseOfFail = "Война за территорию отменена по техническим причинам";

		const int finishGangWarDelayBetweenWinnerChecks = 5000;
		const int startGangWarDelayBetweenInitsOnError = 5000;


		private static GangWarEvent gangWarEvent = new Colshape.GangWarEvent();

		private static readonly NLog.Logger Logger = Gamemode.Logger.Logger.LogFactory.GetLogger("GangWarService");

		public static async Task FinishGangWarAsFailed()
		{
			try
			{
				Rpc.GangWar.FinishRequest finishRequest = new Rpc.GangWar.FinishRequest();
				finishRequest.Failed = true;

				await Infrastructure.RpcClients.GangWarService.FinishAsync(finishRequest);
			}
			catch
			{
				Logger.Error("Finishing gang war failed");
			}
		}

		public static async Task<GangWar> FinishGangWar(byte? winnerFractionID, ICollection<GangWarStatistics>? gangWarStatistics)
		{
			try
			{
				Rpc.GangWar.FinishRequest finishRequest = new Rpc.GangWar.FinishRequest();
				if (winnerFractionID != null) finishRequest.WinnerFractionID = winnerFractionID.Value;
				if (gangWarStatistics != null) finishRequest.GangWarStatistics.AddRange(gangWarStatistics);

				FinishResponse finishResponse = await Infrastructure.RpcClients.GangWarService.FinishAsync(finishRequest);
				return finishResponse.GangWar;
			}
			catch
			{
				Logger.Error("Finishing gang war failed");
			}

			return null;
		}

		public static async Task InitGangWar()
		{
			if (GangWarCache.IsInited() || GangWarCache.IsInProgress()) return;

			StartResponse startResponse;

			try
			{
				startResponse = await Infrastructure.RpcClients.GangWarService.StartAsync(new StartRequest());
			}
			catch
			{
				Logger.Error("Initing gang war failed");
				return;
			}

			GangWarCache.InitGangWarCache(startResponse.GangWar);
			GangZoneCache.MarkAsWarInProgress(startResponse.GangWar.ZoneID);

			NAPI.Task.Run(() =>
			{
				NAPI.Chat.SendChatMessageToAll(String.Format(initGangWarChatMessage, startResponse.GangWar.TargetFractionName));

				ZoneService.StartCapture(startResponse.GangWar.ZoneID);
			});

			Logger.Info("Inited gang war");
		}

		public static async Task StartGangWar(DateTime finishTime)
		{
			if (!GangWarCache.IsInited() || GangWarCache.IsInProgress()) return;
			GangWarCache.SetAsInProgress();

			NAPI.Task.Run(() =>
			{
				NAPI.Chat.SendChatMessageToAll(String.Format(startGangWarChatMessage, GangWarCache.GangWar.TargetFractionName));

				GangWarCache.ColShape = NAPI.ColShape.Create2DColShape(GangWarCache.GangWar.XCoordinate, GangWarCache.GangWar.YCoordinate, 100f, 100f);
				GangWarCache.ColShape.OnEntityEnterColShape += gangWarEvent.OnEntityEnterColShape;
				GangWarCache.ColShape.OnEntityExitColShape += gangWarEvent.OnEntityExitColShape;

				GangWarCache.FinishTime = finishTime;

				List<CustomPlayer> players = new List<CustomPlayer>();
				foreach (CustomPlayer player in NAPI.Pools.GetAllPlayers())
				{
					if (player.LoggedInAt == null) continue;
					players.Add(player);

				}

				NAPI.ClientEvent.TriggerClientEventToPlayers(players.ToArray(), "InitGangWarUI", GangWarCache.RemainingMs().ToString(), GangWarCache.GangWar.TargetFractionID);
			});

			Logger.Info("Started gang war");
		}

		public static async Task FinishGangWar()
		{
			if (!GangWarCache.IsInProgress() || GangWarCache.IsFinishing()) return;
			GangWarCache.SetAsFinishing();

			GangWar gangWar;

			if (GangWarCache.IsZeroKills())
			{
				gangWar = await GangWarService.FinishGangWar(null, null);
			}
			else
			{
				while (true)
				{
					byte? winner = GangWarCache.GetWinner();

					if (winner != null)
					{
						gangWar = await GangWarService.FinishGangWar(winner.Value, GangWarCache.GetStatistics());
						break;
					}

					Logger.Debug("Finishing gang war");
					await Task.Delay(finishGangWarDelayBetweenWinnerChecks);
				}
			}

			if (gangWar == null)
			{
				gangWar = GangWarCache.GangWar;
			}

			long winnerFractionID = gangWar.TargetFractionID;
			if (gangWar.HasWinnerFractionID) winnerFractionID = gangWar.WinnerFractionID;

			GangZoneCache.MarkAsWarFinished(gangWar.ZoneID, winnerFractionID);

			NAPI.Task.Run(() =>
			{
				string message = finishGangWarChatMessage2;
				if (!gangWar.HasWinnerFractionID || gangWar.WinnerFractionID == gangWar.TargetFractionID)
				{
					message = finishGangWarChatMessage3;
				}

				if (gangWar.WinnerFractionName == null)
				{
					NAPI.Chat.SendChatMessageToAll(finishGangWarBecauseOfFail);
				}
				else
				{
					NAPI.Chat.SendChatMessageToAll(String.Format(finishGangWarChatMessage1 + message, gangWar.WinnerFractionName));
				}

				ZoneService.FinishCapture(gangWar.ZoneID, winnerFractionID);
				NAPI.ColShape.DeleteColShape(GangWarCache.ColShape);

				foreach (CustomPlayer player in GangWarCache.PlayersInZone)
				{
					player.IsInWarZone = false;
				}

				NAPI.ClientEvent.TriggerClientEventForAll("CloseGangWarUI");
			});

			await NAPI.Task.WaitForMainThread();

			GangWarCache.ResetGangWarCache();
			Logger.Info("Finished gang war");
		}

		public static void DisplayGangWarUI(CustomPlayer player)
		{
			if (!GangWarCache.IsInProgress()) return;

			GangWarStats gangWarStats = GangWarCache.GetGangWarStats();
			NAPI.ClientEvent.TriggerClientEvent(player, "InitGangWarUI", GangWarCache.RemainingMs().ToString(), GangWarCache.GangWar.TargetFractionID);
			NAPI.ClientEvent.TriggerClientEvent(player, "UpdateGangWarStats", gangWarStats.Ballas, gangWarStats.Bloods, gangWarStats.Marabunta, gangWarStats.Families, gangWarStats.Vagos);
		}
	}
}
