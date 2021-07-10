using System;
using Gamemode.Models.Player;
using GTANetworkAPI;
using Rpc.Report;

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


			CreateRequest createRequest = new CreateRequest();
			createRequest.Question = message;
			createRequest.UserID = player.StaticId;

			CreateResponse createResponse;

			try
			{
				createResponse = await Infrastructure.RpcClients.ReportService.CreateAsync(createRequest);
			}
			catch (Exception)
			{
				NAPI.Task.Run(() => player.SendChatMessage($"Что-то пошло не так, попробуйте еще раз."));
				return;
			}

			NAPI.Task.Run(() =>
			{
				player.SendChatMessage($"Репорт номер {createResponse.Report.ID} был доставлен администрации.");
				AdminsCache.SendMessageToAllAdminsReport($"{player.Name} [{player.Id}]: {message}");
			});
		}
	}
}
