// <copyright file="AdminChatCommand.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Mechanics.Admin.Commands
{
	using Gamemode.Mechanics.Admin.Models;
	using Gamemode.Mechanics.Chat;
	using Gamemode.Mechanics.Player.Models;
	using Gamemode.Mechanics.Utils;
	using GTANetworkAPI;

	public class AdminChat : BaseHandler
	{
		private const string AdminChatUsage = "Использование: /adminchat {сообщение}. Пример: /ac Привет коллеги!";

		[Command("adminchat", AdminChatUsage, Alias = "ac", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void OnAdminChat(CPlayer admin, string? message = null)
		{
			if (message == null)
			{
				admin.SendChatMessage(AdminChatUsage);
				return;
			}

			Cache.SendMessageToAllAdminsChat($"{admin.Name}: {message}");
		}

		private const string AdminAnnouncementUsage = "Использование: /adminannouncement {сообщение}. Пример: /aa Уважаемые игроки...";

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
