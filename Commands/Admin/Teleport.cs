// <copyright file="Teleport.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using GTANetworkAPI;

    public class Teleport : Script
    {
        [Command("teleport", "Usage: /teleport {player_id}", Alias = "tp", SensitiveInfo = true, GreedyArg = true)]
        public void CMDVehicle(Player player, string playerID)
        {

            Player playerTo = PlayerUtil.GetById(ushort.Parse(playerID));
            if (playerTo == null)
            {
                player.SendChatMessage("Player with specified ID does not exist");
                return;
            }

            player.Position = playerTo.Position;
        }
    }
}
