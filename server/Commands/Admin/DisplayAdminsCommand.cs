// <copyright file="DisplayAdminsCommand.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Commands.Admin
{
	using Gamemode.Models.Admin;
	using Gamemode.Models.Player;
	using GTANetworkAPI;

	public class DisplayAdminsCommand : Script
	{
		private const string DisplayAdminsCommandUsage = "Использование: /admins";

		[Command("admins", DisplayAdminsCommandUsage, Alias = "a", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void DisplayAdmins(CustomPlayer admin)
		{
			admin.SendChatMessage($"Админы онлайн: {AdminsCache.GetAdminNames()}");
		}
	}
}
