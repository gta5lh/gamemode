// <copyright file="Position.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using Gamemode.Commands.Admin;
    using Gamemode.Models.Admin;
    using Gamemode.Models.Player;
    using GTANetworkAPI;

    public class Position : Script
    {
        [Command("position", "Usage: /position {player_id}", Alias = "pos", SensitiveInfo = true, GreedyArg = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public void CMDPosition(CustomPlayer admin, string playerID)
        {
            admin.SendChatMessage($"X: {admin.Position.X}, Y: {admin.Position.Y}, Z: {admin.Position.Z}, Heading: {admin.Heading}");
        }
    }
}
