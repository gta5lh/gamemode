﻿// <copyright file="ChatController.cs" company="lbyte00">
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
            if (!Services.Player.ChatService.CheckMute(player)) return;

            NAPI.Chat.SendChatMessageToAll(string.Format("{0}{1} [{2}]:~s~ {3}", player.ChatColor, player.Name, player.Id, message));
        }
    }
}
