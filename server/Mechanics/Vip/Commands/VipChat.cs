﻿using GTANetworkAPI;
using Gamemode.Mechanics.Player.Models;

namespace Gamemode.Mechanics.Vip.Commands
{
	public class VipChat : Script
	{
		private const string VipChatUsage = "Использование: /vipchat {сообщение}. Пример: /vc Всем привет!";

		[Command("vipchat", VipChatUsage, Alias = "vc", GreedyArg = true)]
		public void OnVipChat(CPlayer player, string? message = null)
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
