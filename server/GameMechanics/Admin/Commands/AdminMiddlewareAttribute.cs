// <copyright file="AdminMiddlewareAttribute.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.GameMechanics.Admin.Commands
{
	using Gamemode.GameMechanics.Admin.Models;
	using Gamemode.GameMechanics.Player.Models;
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
