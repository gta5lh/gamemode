// <copyright file="AdminMiddlewareAttribute.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Commands.Admin
{
    using Gamemode.Models.Admin;
    using Gamemode.Models.Player;
    using GTANetworkAPI;

    public class AdminMiddlewareAttribute : CommandConditionAttribute
    {
        private readonly AdminRank atLeast;

        public AdminMiddlewareAttribute(AdminRank rankAtLeast)
        {
            this.atLeast = rankAtLeast;
        }

        public override bool Check(Player player, string cmdName, string cmdText)
        {
            return ((CustomPlayer)player).AdminRank.AtLeast(this.atLeast);
        }
    }
}
