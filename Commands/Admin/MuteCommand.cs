// <copyright file="Mute.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Commands.Admin
{
    using System;
    using Gamemode.Utils;
    using GTANetworkAPI;

    public class MuteCommand : Script
    {
        [Command("mute", "Usage: /mute {static_id} {минуты} {причина}", Alias = "m", SensitiveInfo = true, GreedyArg = true)]
        public void Mute(Player player, string playerID, string durationMinutes, string reason)
        {
            int duration;

            try
            {
                duration = int.Parse(durationMinutes);
            }
            catch (Exception)
            {
                NAPI.Chat.SendChatMessageToPlayer(player, Resources.Localization.ErrorMuteMinutesDuration);
                return;
            }

            Player targetPlayer = PlayerUtil.GetByID(playerID);
            PlayerCache.GetPlayerCache(targetPlayer).MuteState.Mute(duration);
            Chat.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, ChatMessage.AnnouncementAdminMutedPlayer(player.Name, targetPlayer.Name, duration, reason));
        }
    }
}
