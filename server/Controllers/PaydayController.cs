﻿namespace Gamemode.Controllers
{
	using System;
	using System.Timers;
	using Gamemode.Models.Player;
	using Gamemode.Services.Player;
	using GamemodeCommon.Models;
	using GTANetworkAPI;

	public class PaydayController : Script
	{

		private static Timer PaydayTimer;

		// private static readonly double PaydayInterval30Minutes = 1000 * 60 * 30;
		// private static readonly double PaydayAllowedLeeway = -(1000 * 60);
		private static readonly double PaydayInterval30Minutes = 20000;
		private static readonly double PaydayAllowedLeeway = -2000;

		public static void InitPaydayTimer()
		{
			PaydayTimer = new System.Timers.Timer(PaydayInterval30Minutes);
			PaydayTimer.Elapsed += OnPaydayTime;
			PaydayTimer.AutoReset = true;
			PaydayTimer.Start();
		}

		private static async void OnPaydayTime(object source, ElapsedEventArgs e)
		{
			NAPI.Task.Run(async () =>
			{
				DateTime paydayTime = DateTime.UtcNow.AddMilliseconds(PaydayAllowedLeeway);

				foreach (CustomPlayer player in NAPI.Pools.GetAllPlayers())
				{
					if (player.Fraction == null || player.LoggedInAt == null)
					{
						continue;
					}

					if (DateTime.Compare(player.LoggedInAt.Value.AddMilliseconds(PaydayInterval30Minutes), paydayTime) >= 1)
					{
						continue;
					}

					long salary = GangUtil.SalaryByRank[player.FractionRank.Value];
					player.Money += salary;
					player.SendNotification($"[Payday] На счет поступило: {salary} $", 0, 5000, NotificationType.Success);
					await ExperienceService.ChangeExperience(player, 3);
				}
			});
		}
	}
}
