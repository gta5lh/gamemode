namespace Gamemode.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Timers;
	using Gamemode.ApiClient;
	using Gamemode.ApiClient.Models;
	using Gamemode.Models.Player;
	using GTANetworkAPI;

	public class SaveUsersController : Script
	{
		private static readonly NLog.Logger Logger = Gamemode.Logger.Logger.LogFactory.GetLogger("SaveUsersController");

		private static Timer SaveUsersTimer;

		private static readonly double SaveUserInterval10Seconds = 30000;

		public static void IniSaveUserTimer()
		{
			SaveUsersTimer = new System.Timers.Timer(SaveUserInterval10Seconds);
			SaveUsersTimer.Elapsed += OnSaveUser;
			SaveUsersTimer.AutoReset = true;
			SaveUsersTimer.Start();
		}

		private static async void OnSaveUser(object source, ElapsedEventArgs e)
		{
			List<CustomPlayer> players = null;

			NAPI.Task.Run(() =>
			{
				players = NAPI.Pools.GetAllPlayers().Cast<CustomPlayer>().Where(p => p.LoggedInAt != null).ToList();
			});

			await NAPI.Task.WaitForMainThread();

			if (players == null || players.Count == 0)
			{
				Logger.Debug("skipping users save: no users online");
				return;
			}

			List<SaveUserRequest> saveUserRequests = new List<SaveUserRequest>();

			NAPI.Task.Run(() =>
			{
				foreach (CustomPlayer player in players)
				{
					saveUserRequests.Add(new SaveUserRequest(player.StaticId, player.CurrentExperience, player.GetAllWeapons(), player.Money));
				}
			});

			await NAPI.Task.WaitForMainThread();

			SaveUsersRequest saveUsersRequest = new SaveUsersRequest(saveUserRequests);

			try
			{
				await ApiClient.SaveUsers(saveUsersRequest);
			}
			catch (Exception ex)
			{
				Logger.Error(ex.Message);
			}
		}
	}
}
