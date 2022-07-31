// <copyright file="VipChat.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Vip.Commands
{
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class VipChat : Script
	{
		private const string VipChatUsage = "Использование: /vipchat {сообщение}. Пример: /vc Всем привет!";

		[Command("vipchat", VipChatUsage, Alias = "vc", GreedyArg = true)]
		public static void OnVipChat(CPlayer player, string? message = null)
		{
			if (message == null)
			{
				player.SendChatMessage(VipChatUsage);
				return;
			}

			Cache.SendMessageToAllVipsChat($"{player.Name} [{player.Id}]: {message}", true);
		}
	}
}
