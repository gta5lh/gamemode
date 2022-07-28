using System;
using GTANetworkAPI;
using Gamemode.Mechanics.Admin.Models;
using Gamemode.Mechanics.Player.Models;
using Gamemode.Mechanics.Utils;
using Gamemode.Mechanics.Chat;

namespace Gamemode.Mechanics.Admin.Commands
{
	public class Report : BaseHandler
	{
		private const string ReportAnswerUsage = "Использование: /reportanswer. Пример: /ra";

		[Command("reportanswer", ReportAnswerUsage, Alias = "ra", GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void OnReportAnswer(CPlayer admin, string? targetIdInput = null, string? message = null)
		{
			if (targetIdInput == null || message == null)
			{
				admin.SendChatMessage(ReportAnswerUsage);
				return;
			}

			ushort playerId = 0;

			try
			{
				playerId = ushort.Parse(targetIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(ReportAnswerUsage);
				return;
			}

			CPlayer targetPlayer = PlayerUtil.GetById(playerId);
			if (targetPlayer == null)
			{
				admin.SendChatMessage($"Пользователь с DID {playerId} не найден");
				return;
			}

			targetPlayer.SendChatMessage($"{ChatColor.AdminReportAnswerColor}Администратор {admin.Name} ответил вам: {message}");
			Cache.SendMessageToAllAdminsReportAnswer($"{admin.Name} ответил {targetPlayer.Name} [{targetPlayer.Id}]: {message}");
		}
	}
}
