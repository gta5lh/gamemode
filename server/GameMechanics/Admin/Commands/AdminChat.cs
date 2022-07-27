// <copyright file="AdminChatCommand.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.GameMechanics.Admin.Commands
{
	using Gamemode.GameMechanics.Admin.Models;
	using Gamemode.GameMechanics.Player.Models;
	using Gamemode.Utils;
	using GTANetworkAPI;

	public class AdminChat : Script
	{
		private const string AdminChatUsage = "Использование: /adminchat {сообщение}. Пример: /ac Привет коллеги!";
		private const string AdminAnnouncementUsage = "Использование: /adminannouncement {сообщение}. Пример: /aa Уважаемые игроки...";

		[Command("adminchat", AdminChatUsage, Alias = "ac", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void OnAdminChat(CPlayer admin, string? message = null)
		{
			if (message == null)
			{
				admin.SendChatMessage(AdminChatUsage);
				return;
			}

			// TODO
			// AdminsCache.SendMessageToAllAdminsChat($"{admin.Name}: {message}");
		}

		[Command("adminannouncement", AdminAnnouncementUsage, Alias = "aa", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void OnAdminAnnouncement(CPlayer admin, string? message = null)
		{
			if (message == null)
			{
				admin.SendChatMessage(AdminAnnouncementUsage);
				return;
			}

			NAPI.Chat.SendChatMessageToAll($"{ChatColor.AdminAnnouncementColor}Администратор {admin.Name}: {message}");
		}
	}
}
