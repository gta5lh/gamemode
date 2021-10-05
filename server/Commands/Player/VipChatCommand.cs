using System;
using System.Threading.Tasks;
using Gamemode.Models.Player;
using GTANetworkAPI;
using Rpc.Report;
using Gamemode.Cache.Player;

namespace Gamemode.Commands.Player
{
	public class VipChatCommand : Script
	{
		private const string VipChatCommandUsage = "Использование: /vipchat {сообщение}. Пример: /vc Всем привет!";

		[Command("vipchat", VipChatCommandUsage, Alias = "vc", GreedyArg = true)]
		public void VipChat(CustomPlayer player, string? message = null)
		{
			if (message == null)
			{
				player.SendChatMessage(VipChatCommandUsage);
				return;
			}

			VipsCache.SendMessageToAllVipsChat($"{player.Name} [{player.Id}]: {message}", true);
		}
	}
}
