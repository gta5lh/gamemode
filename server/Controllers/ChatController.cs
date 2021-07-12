// <copyright file="ChatController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
	using System.Threading.Tasks;
	using Gamemode.Models.Player;
	using Gamemode.Services.Player;
	using GTANetworkAPI;

	public class ChatController : Script
	{
		[ServerEvent(Event.ChatMessage)]
		private async void ChatMessage(CustomPlayer player, string message)
		{
			if (!await ChatService.CheckMute(player)) return;

			NAPI.Task.Run(() =>
			{
				NAPI.Chat.SendChatMessageToAll(string.Format("{0}{1} [{2}]:~s~ {3}", player.ChatColor, player.Name, player.Id, message));
			});
		}
	}
}
