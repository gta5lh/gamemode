// <copyright file="DisplayAdmins.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Admin.Commands
{
	using Gamemode.Game.Admin.Models;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class DisplayAdmins : Script
	{
		private const string DisplayAdminsUsage = "Использование: /admins";

		[Command("admins", DisplayAdminsUsage, Alias = "a", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public static void OnDisplayAdmins(CPlayer admin)
		{
			admin.SendChatMessage($"Админы онлайн: {Cache.GetAdminNames()}");
		}
	}
}
