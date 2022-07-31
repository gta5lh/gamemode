// <copyright file="AdminMiddlewareAttribute.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Admin.Commands
{
	using Gamemode.Game.Admin.Models;
	using Gamemode.Game.Player.Models;
	using Gamemode.Game.ServerSettings;
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
			CPlayer cPlayer = (CPlayer)player;

			if (!Settings.IsProduction() && !cPlayer.AdminRank.IsAdmin())
			{
				player.SendChatMessage($"Admin commands available to all players on development environment. Current={cPlayer.AdminRank} Required={this.atLeast}");
				return true;
			}

			return cPlayer.AdminRank.AtLeast(this.atLeast);
		}
	}
}
