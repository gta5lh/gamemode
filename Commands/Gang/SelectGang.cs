// <copyright file="SelectGang.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using GTANetworkAPI;

    public class SelectGang : Script
    {
        [Command("sgang", "Usage: /sgang {gang_id}", SensitiveInfo = true, GreedyArg = true)]
        public void CMDSelectGang(Player player, string gangName)
        {
            // Method intentionally left empty.
        }

        [Command("mgang", "Usage: /sgang {gang_id}", SensitiveInfo = true, GreedyArg = true)]
        public void CMDMyGang(Player player, string gangID)
        {
            NAPI.Chat.SendChatMessageToPlayer(player, player.GetData<string>("gang_type"));
        }
    }
}
