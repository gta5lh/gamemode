using System;
using Gamemode.Models.Admin;
using Gamemode.Models.Player;
using GTANetworkAPI;
using Rpc.GameServer;

namespace Gamemode.Commands.Admin
{
	public class TimeCommand : BaseCommandHandler
	{
		private const string SetTimeCommandUsage = "Использование: /sett {час} {минуты}. Пример: /sett 23 59";

		[Command("settime", SetTimeCommandUsage, Alias = "sett", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Owner)]
		public async void SetTime(CustomPlayer admin, string hoursInput = null, string minutesInput = null)
		{
			if (hoursInput == null || minutesInput == null)
			{
				admin.SendChatMessage(SetTimeCommandUsage);
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
				admin.SendChatMessage(SetTimeCommandUsage);
				return;
			}

			if (hours > 24 || hours < 0 || minutes > 60 || minutes < 0)
			{
				admin.SendChatMessage(SetTimeCommandUsage);
				return;
			}

			TimeController.StopTimeSync();

			TimeSpan time = new TimeSpan(hours, minutes, 00);
			TimeController.SetCurrentTime(time);

			try
			{
				Infrastructure.RpcClients.GameServerService.SetTimeAsync(new SetTimeRequest(admin.StaticId, admin.Name, hours, minutes));
			}
			catch { }


			AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} установил время на {hours:00.##}:{minutes:00.##}:00");
			this.Logger.Warn($"Administrator {admin.Name} set time to {hours:00.##}:{minutes:00.##}:00");
		}

		private const string SyncTimeCommandUsage = "Использование: /synct";

		[Command("synctime", SetTimeCommandUsage, Alias = "synct", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Owner)]
		public async void SyncTime(CustomPlayer admin)
		{
			TimeController.StartTimeSync();

			try
			{
				Infrastructure.RpcClients.GameServerService.SyncTimeAsync(new SyncTimeRequest(admin.StaticId, admin.Name));
			}
			catch { }

			AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} возобновил синхронизацию серверного время с GMT+3");
			this.Logger.Warn($"Administrator {admin.Name} restored time synchronization with GMT+3");
		}

		private const string GetTimeCommandUsage = "Использование: /gett";

		[Command("gettime", SetTimeCommandUsage, Alias = "gett", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Owner)]
		public void GetTime(CustomPlayer admin)
		{
			TimeSpan currentTime = TimeController.CurrentTime;
			admin.SendChatMessage($"Текущее время на сервере: {currentTime.Hours:00.##}:{currentTime.Minutes:00.##}:{currentTime.Seconds:00.##}");
		}
	}
}
