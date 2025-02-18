﻿// <copyright file="Time.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Admin.Commands
{
	using System;
	using Gamemode.Game.Admin.Models;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;
	using Rpc.GameServer;

	public class Time : BaseHandler
	{
		private const string SetTimeUsage = "Использование: /sett {час} {минуты}. Пример: /sett 23 59";
		private const string SyncTimeUsage = "Использование: /synct";
		private const string GetTimeUsage = "Использование: /gett";

		[Command("gettime", GetTimeUsage, Alias = "gett", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Owner)]
		public static void OnGetTime(CPlayer admin)
		{
			TimeSpan currentTime = Game.Time.Controllers.Time.CurrentTime;
			admin.SendChatMessage($"Текущее время на сервере: {currentTime.Hours:00.##}:{currentTime.Minutes:00.##}:{currentTime.Seconds:00.##}");
		}

		[Command("settime", SetTimeUsage, Alias = "sett", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Owner)]
		public async System.Threading.Tasks.Task OnSetTime(CPlayer admin, string? hoursInput = null, string? minutesInput = null)
		{
			if (hoursInput == null || minutesInput == null)
			{
				admin.SendChatMessage(SetTimeUsage);
				return;
			}

			int hours;
			int minutes;

			try
			{
				hours = int.Parse(hoursInput);
				minutes = int.Parse(minutesInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(SetTimeUsage);
				return;
			}

			if (hours > 24 || hours < 0 || minutes > 60 || minutes < 0)
			{
				admin.SendChatMessage(SetTimeUsage);
				return;
			}

			Game.Time.Controllers.Time.StopTimeSync();

			TimeSpan time = new(hours, minutes, 00);
			Game.Time.Controllers.Time.SetCurrentTime(time);

			try
			{
				await Infrastructure.RpcClients.GameServerService.SetTimeAsync(new SetTimeRequest(admin.StaticId, admin.Name, hours, minutes));
			}
			catch
			{
				NAPI.Task.Run(() => admin.SendChatMessage("Установить время неполучилось!"));
				return;
			}

			NAPI.Task.Run(() =>
			{
				Cache.SendMessageToAllAdminsAction($"{admin.Name} установил время на {hours:00.##}:{minutes:00.##}:00");
				this.Logger.Warn($"Administrator {admin.Name} set time to {hours:00.##}:{minutes:00.##}:00");
			});
		}

		[Command("synctime", SyncTimeUsage, Alias = "synct", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Owner)]
		public async System.Threading.Tasks.Task OnSyncTime(CPlayer admin)
		{
			Game.Time.Controllers.Time.StartTimeSync();

			try
			{
				await Infrastructure.RpcClients.GameServerService.SyncTimeAsync(new SyncTimeRequest(admin.StaticId, admin.Name));
			}
			catch
			{
				NAPI.Task.Run(() => admin.SendChatMessage("Синхронизировать время неполучилось!"));
				return;
			}

			NAPI.Task.Run(() =>
			{
				Cache.SendMessageToAllAdminsAction($"{admin.Name} возобновил синхронизацию серверного времени с GMT+3");
				this.Logger.Warn($"Administrator {admin.Name} restored time synchronization with GMT+3");
			});
		}
	}
}
