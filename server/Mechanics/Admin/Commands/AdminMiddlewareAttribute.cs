// <copyright file="AdminMiddlewareAttribute.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Mechanics.Admin.Commands
{
	using Gamemode.Mechanics.Admin.Models;
	using Gamemode.Mechanics.Player.Models;
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
			return ((CPlayer)player).AdminRank.AtLeast(this.atLeast);
		}
	}
}
