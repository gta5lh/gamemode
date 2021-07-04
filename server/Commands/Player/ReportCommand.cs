using System;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Commands.Player
{
	public class ReportCommand : Script
	{
		private const string ReportCommandUsage = "Использование: /report {сообщение}. Пример: /r ИД 10 читер";

		[Command("report", ReportCommandUsage, Alias = "r", GreedyArg = true)]
		public async void ReportAsync(CustomPlayer player, string? message = null)
		{
			if (message == null)
			{
				player.SendChatMessage(ReportCommandUsage);
				return;
			}

			Report report = new Report(player.StaticId, message);

			try
			{
				report = await ApiClient.ApiClient.CreateReport(report);
			}
			catch (Exception)
			{
				NAPI.Task.Run(() => player.SendChatMessage($"Что-то пошло не так, попробуйте еще раз."));
				return;
			}

			NAPI.Task.Run(() =>
			{
				player.SendChatMessage($"Репорт номер {report.Id} был доставлен администрации.");
				AdminsCache.SendMessageToAllAdminsReport($"{player.Name} [{player.Id}]: {message}");
			});
		}
	}
}
