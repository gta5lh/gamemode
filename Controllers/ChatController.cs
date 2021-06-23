// <copyright file="ChatController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
	using Gamemode.Models.Player;
	using GTANetworkAPI;

	public class ChatController : Script
	{
		[ServerEvent(Event.ChatMessage)]
		private void ChatMessage(CustomPlayer player, string message)
		{
			bool isMuted = player.MuteState != null && player.MuteState.IsMuted();
			if (isMuted && player.MuteState.HasMuteExpired())
			{
				player.Unmute();
				player.SendChatMessage("Срок действия вашего мута истек. Не нарушайте правила сервера. Приятной игры!");
			}
			else if (isMuted)
			{
				player.SendChatMessage($"Администратор выдал вам мут. Осталось {player.MuteState.GetMinutesLeft():0.##} минут.");
				return;
			}

			NAPI.Chat.SendChatMessageToAll(string.Format("{0}{1} [{2}]:~s~ {3}", player.ChatColor, player.Name, player.Id, message));
		}
	}
}
