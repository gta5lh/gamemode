namespace Gamemode.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading.Tasks;
	using System.Timers;
	using Gamemode.ApiClient;
	using Gamemode.ApiClient.Models;
	using Gamemode.Models.Player;
	using Gamemode.Services.Player;
	using GTANetworkAPI;

	public class SavePlayersController : Script
	{
		private static readonly NLog.Logger Logger = Gamemode.Logger.Logger.LogFactory.GetLogger("SavePlayersController");

		private static Timer SavePlayersTimer;

		private static readonly double SavePlayerInterval120Seconds = 120000;

		public static void InitSavePlayerTimer()
		{
			SavePlayersTimer = new System.Timers.Timer(SavePlayerInterval120Seconds);
			SavePlayersTimer.Elapsed += OnSavePlayers;
			SavePlayersTimer.AutoReset = true;
			SavePlayersTimer.Start();
		}

		private static async void OnSavePlayers(object source, ElapsedEventArgs e)
		{
			List<CustomPlayer> players = null;

			NAPI.Task.Run(() =>
			{
				players = NAPI.Pools.GetAllPlayers().Cast<CustomPlayer>().Where(p => p.LoggedInAt != null).ToList();
			});

			await NAPI.Task.WaitForMainThread();

			if (players == null || players.Count == 0)
			{
				Logger.Debug("skipping players save: no players online");
				return;
			}

			await PlayerService.SavePlayers(players);
		}
	}
}
