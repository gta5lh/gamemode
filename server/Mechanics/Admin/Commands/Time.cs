using System;
using GTANetworkAPI;
using Rpc.GameServer;
using Gamemode.Mechanics.Admin.Models;
using Gamemode.Mechanics.Player.Models;

namespace Gamemode.Mechanics.Admin.Commands
{
	public class Time : BaseHandler
	{
		private const string SetTimeUsage = "Использование: /sett {час} {минуты}. Пример: /sett 23 59";

		[Command("settime", SetTimeUsage, Alias = "sett", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Owner)]
		public async void OnSetTime(CPlayer admin, string? hoursInput = null, string? minutesInput = null)
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

			Mechanics.Time.Controllers.Time.StopTimeSync();

			TimeSpan time = new TimeSpan(hours, minutes, 00);
			Mechanics.Time.Controllers.Time.SetCurrentTime(time);

			try
			{
				await Infrastructure.RpcClients.GameServerService.SetTimeAsync(new SetTimeRequest(admin.StaticId, admin.Name, hours, minutes));
			}
			catch { }


			Cache.SendMessageToAllAdminsAction($"{admin.Name} установил время на {hours:00.##}:{minutes:00.##}:00");
			this.Logger.Warn($"Administrator {admin.Name} set time to {hours:00.##}:{minutes:00.##}:00");
		}

		private const string SyncTimeUsage = "Использование: /synct";

		[Command("synctime", SetTimeUsage, Alias = "synct", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Owner)]
		public async void OnSyncTime(CPlayer admin)
		{
			Mechanics.Time.Controllers.Time.StartTimeSync();

			try
			{
				await Infrastructure.RpcClients.GameServerService.SyncTimeAsync(new SyncTimeRequest(admin.StaticId, admin.Name));
			}
			catch { }

			Cache.SendMessageToAllAdminsAction($"{admin.Name} возобновил синхронизацию серверного время с GMT+3");
			this.Logger.Warn($"Administrator {admin.Name} restored time synchronization with GMT+3");
		}

		private const string GetTimeUsage = "Использование: /gett";

		[Command("gettime", SetTimeUsage, Alias = "gett", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Owner)]
		public void OnGetTime(CPlayer admin)
		{
			TimeSpan currentTime = Mechanics.Time.Controllers.Time.CurrentTime;
			admin.SendChatMessage($"Текущее время на сервере: {currentTime.Hours:00.##}:{currentTime.Minutes:00.##}:{currentTime.Seconds:00.##}");
		}
	}
}
