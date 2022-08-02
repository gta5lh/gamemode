// <copyright file="AdminAnnouncement.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Admin.Commands
{
	using Gamemode.Game.Admin.Models;
	using Gamemode.Game.Chat;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class AdminAnnouncement : BaseHandler
	{
		private const string AdminAnnouncementUsage = "Использование: /adminannouncement {сообщение}. Пример: /aa Уважаемые игроки...";

		[Command("adminannouncement", AdminAnnouncementUsage, Alias = "aa", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public static void OnAdminAnnouncement(CPlayer admin, string? message = null)
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
