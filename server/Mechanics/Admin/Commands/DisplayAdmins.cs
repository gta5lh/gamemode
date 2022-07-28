// <copyright file="DisplayAdminsCommand.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Mechanics.Admin.Commands
{
	using Gamemode.Mechanics.Admin.Models;
	using Gamemode.Mechanics.Player.Models;
	using GTANetworkAPI;

	public class DisplayAdmins : Script
	{
		private const string DisplayAdminsUsage = "Использование: /admins";

		[Command("admins", DisplayAdminsUsage, Alias = "a", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void OnDisplayAdmins(CPlayer admin)
		{
			admin.SendChatMessage($"Админы онлайн: {Cache.GetAdminNames()}");
		}
	}
}
