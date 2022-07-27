using System;
using GTANetworkAPI;
using Gamemode.GameMechanics.Admin.Models;
using Gamemode.GameMechanics.Player.Models;

namespace Gamemode.GameMechanics.Admin.Commands
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

			// TODO
			// CPlayer targetPlayer = PlayerUtil.GetById(playerId);
			// if (targetPlayer == null)
			// {
			// 	admin.SendChatMessage($"Пользователь с DID {playerId} не найден");
			// 	return;
			// }

			// targetPlayer.SendChatMessage($"{ChatColor.AdminReportAnswerColor}Администратор {admin.Name} ответил вам: {message}");
			// AdminsCache.SendMessageToAllAdminsReportAnswer($"{admin.Name} ответил {targetPlayer.Name} [{targetPlayer.Id}]: {message}");
		}
	}
}
