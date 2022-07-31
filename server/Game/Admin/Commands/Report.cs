// <copyright file="Report.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Admin.Commands
{
	using System;
	using Gamemode.Game.Admin.Models;
	using Gamemode.Game.Chat;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class Report : BaseHandler
	{
		private const string ReportAnswerUsage = "Использование: /reportanswer. Пример: /ra";

		[Command("reportanswer", ReportAnswerUsage, Alias = "ra", GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public static void OnReportAnswer(CPlayer admin, string? targetIdInput = null, string? message = null)
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
