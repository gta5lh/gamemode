// <copyright file="AdminChat.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Admin.Commands
{
	using Gamemode.Game.Admin.Models;
	using Gamemode.Game.Chat;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class AdminChat : BaseHandler
	{
		private const string AdminChatUsage = "Использование: /adminchat {сообщение}. Пример: /ac Привет коллеги!";

		[Command("adminchat", AdminChatUsage, Alias = "ac", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public static void OnAdminChat(CPlayer admin, string? message = null)
		{
			if (message == null)
			{
				admin.SendChatMessage(AdminChatUsage);
				return;
			}

			Cache.SendMessageToAllAdminsChat($"{admin.Name}: {message}");
		}
	}
}
