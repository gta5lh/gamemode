using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
using Gamemode.Cache.GangWar;
using Gamemode.Cache.GangZone;
using Gamemode.Colshape;
using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Services
{
	public static class GangWarService
	{
		const string initGangWarChatMessage1 = "Начинается война за территорию через 10 минут";
		const string initGangWarChatMessage2 = "• Нападение на {0}";
		const string initGangWarChatMessage3 = "• Координаты: X={0}, Y={1}";

		const string startGangWarChatMessage1 = "Началась война за территорию!";
		const string startGangWarChatMessage2 = "• Нападение на {0}";
		const string startGangWarChatMessage3 = "• Координаты: X={0}, Y={1}";

		const string finishGangWarChatMessage1 = "Закончилась война за территорию!";
		const string finishGangWarChatMessage2 = "• Победили {0}";
		const string finishGangWarChatMessage3 = "• Территория осталась во владении {0}";

		const int finishGangWarDelayBetweenWinnerChecks = 5000;


		private static GangWarEvent gangWarEvent = new Colshape.GangWarEvent();

		private static readonly NLog.Logger Logger = Gamemode.Logger.Logger.LogFactory.GetLogger("GangWarService");

		public static async Task FinishGangWarAsFailed()
		{
			try
			{
				await ApiClient.ApiClient.FinishGangWar(new FinishGangWarRequest(true));
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public static async Task<GangWar> FinishGangWar(byte? winnerFractionID, ICollection<GangWarStatistics>? gangWarStatistics)
		{
			try
			{
				return await ApiClient.ApiClient.FinishGangWar(new FinishGangWarRequest(false, winnerFractionID, gangWarStatistics));
			}
			catch (Exception e)
			{
				throw e;
			}
		}

		public static async Task InitGangWar()
		{
			if (GangWarCache.IsInProgress()) return;

			ApiClient.Models.GangWar gangWar = await ApiClient.ApiClient.StartGangWar();
			GangWarCache.InitGangWarCache(gangWar);
			GangZoneCache.MarkAsWarInProgress(gangWar.ZoneID);

			NAPI.Task.Run(() =>
			{
				NAPI.Chat.SendChatMessageToAll(initGangWarChatMessage1);
				NAPI.Chat.SendChatMessageToAll(String.Format(initGangWarChatMessage2, gangWar.TargetFractionName));
				NAPI.Chat.SendChatMessageToAll(String.Format(initGangWarChatMessage3, gangWar.XCoordinate, gangWar.YCoordinate));

				ZoneService.StartCapture(gangWar.ZoneID);
			});

			Logger.Info("Inited gang war");
		}

		public static async Task StartGangWar()
		{
			if (!GangWarCache.IsInited() || GangWarCache.IsInProgress()) return;
			GangWarCache.SetAsInProgress();

			NAPI.Task.Run(() =>
			{
				NAPI.Chat.SendChatMessageToAll(startGangWarChatMessage1);
				NAPI.Chat.SendChatMessageToAll(String.Format(startGangWarChatMessage2, GangWarCache.GangWar.TargetFractionName));
				NAPI.Chat.SendChatMessageToAll(String.Format(startGangWarChatMessage3, GangWarCache.GangWar.XCoordinate, GangWarCache.GangWar.YCoordinate));

				GangWarCache.ColShape = NAPI.ColShape.Create2DColShape(GangWarCache.GangWar.XCoordinate, GangWarCache.GangWar.YCoordinate, 100f, 100f);
				GangWarCache.ColShape.OnEntityEnterColShape += gangWarEvent.OnEntityEnterColShape;
				GangWarCache.ColShape.OnEntityExitColShape += gangWarEvent.OnEntityExitColShape;

			});

			Logger.Info("Started gang war");
		}

		public static async Task FinishGangWar()
		{
			if (!GangWarCache.IsInProgress()) return;

			ApiClient.Models.GangWar gangWar;

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

					await Task.Delay(finishGangWarDelayBetweenWinnerChecks);
				}
			}

			byte winnerFractionID = gangWar.WinnerFractionID != null ? gangWar.WinnerFractionID.Value : gangWar.TargetFractionID;
			GangZoneCache.MarkAsWarFinished(gangWar.ZoneID, winnerFractionID);

			NAPI.Task.Run(() =>
			{
				NAPI.Chat.SendChatMessageToAll(finishGangWarChatMessage1);

				string message = finishGangWarChatMessage2;
				if (gangWar.WinnerFractionID == null || gangWar.WinnerFractionID == gangWar.TargetFractionID)
				{
					message = finishGangWarChatMessage3;
				}

				NAPI.Chat.SendChatMessageToAll(String.Format(message, gangWar.WinnerFractionName));
				ZoneService.FinishCapture(gangWar.ZoneID, winnerFractionID);
				NAPI.ColShape.DeleteColShape(GangWarCache.ColShape);
			});

			GangWarCache.ResetGangWarCache();
			Logger.Info("Finished gang war");
		}
	}
}
