// <copyright file="PlayerUtil.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using GTANetworkAPI;

    public static class PlayerUtil
    {
        public static Player GetByID(string playerID)
        {
            return NAPI.Player.GetPlayerFromHandle(new NetHandle(ushort.Parse(playerID), EntityType.Player));
        }
    }
}
