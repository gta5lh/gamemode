// <copyright file="Position.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using GTANetworkAPI;

    public class Position : Script
    {
        [Command("position", "Usage: /position {player_id}", SensitiveInfo = true, GreedyArg = true)]
        public void CMDPosition(Player player, string playerID)
        {
            Vector3 playerPos = NAPI.Entity.GetEntityPosition(player);
            NAPI.Chat.SendChatMessageToPlayer(player, string.Format("X: {0}, Y: {1}, Z: {2}", playerPos.X, playerPos.Y, playerPos.Z));
        }
    }
}
