// <copyright file="Ped.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using GTANetworkAPI;

    public class Ped : Script
    {
        [Command("ped", "Usage: /ped", SensitiveInfo = true, GreedyArg = true)]
        public void CMDPed(Player player)
        {
            NAPI.Ped.CreatePed(PedHash.Acult01AMM, player.Position, player.Heading);
        }
    }
}
