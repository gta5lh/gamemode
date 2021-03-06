// <copyright file="ChatController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using GTANetworkAPI;

    public class ChatController : Script
    {
        [ServerEvent(Event.ChatMessage)]
        private void ChatMessage(Player player, string message)
        {
            PlayerCache playerCache = PlayerCache.GetPlayerCache(player);

            bool isMuted = playerCache.MuteState.IsMuted();
            if (isMuted && playerCache.MuteState.HasMuteExpired())
            {
                playerCache.MuteState.Unmute();
            }
            else if (isMuted)
            {
                NAPI.Chat.SendChatMessageToPlayer(player, "You are muted!");
                return;
            }

            NAPI.Chat.SendChatMessageToAll(string.Format("{0}{1} [{2}]:~s~ {3}", playerCache.ChatColor, player.Name, player.Id, message));
        }
    }
}
