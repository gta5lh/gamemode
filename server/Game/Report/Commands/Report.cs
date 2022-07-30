// <copyright file="Report.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Report.Commands
{
	using System;
	using System.Threading.Tasks;
	using Gamemode.Game.Admin;
	using Gamemode.Game.Player.Models;
	using Gamemode.Game.ServerSettings;
	using GTANetworkAPI;
	using Rpc.Report;

	public class Report : Script
	{
		private const string ReportUsage = "Использование: /report {сообщение}. Пример: /r ИД 10 читер";

		[Command("report", ReportUsage, Alias = "r", GreedyArg = true)]
		public static async Task OnReportAsync(CPlayer player, string? message = null)
		{
			if (message == null)
			{
				player.SendChatMessage(ReportUsage);
				return;
			}

			CreateRequest createRequest = new CreateRequest();
			createRequest.Question = message;
			createRequest.PlayerID = player.PKId.ToString();
			createRequest.ServerID = Settings.ServerID;

			CreateResponse createResponse;

			try
			{
				createResponse = await Infrastructure.RpcClients.ReportService.CreateAsync(createRequest);
			}
			catch (Exception)
			{
				NAPI.Task.Run(() => player.SendChatMessage("Что-то пошло не так, попробуйте еще раз."));
				return;
			}

			NAPI.Task.Run(() =>
			{
				player.SendChatMessage($"Репорт номер {createResponse.Report.ID} был доставлен администрации.");
				Cache.SendMessageToAllAdminsReport($"{player.Name} [{player.Id}]: {message}");
			});
		}
	}
}
