// <copyright file="DisplayAdminsCommand.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.GameMechanics.Admin.Commands
{
	using Gamemode.GameMechanics.Admin.Models;
	using Gamemode.GameMechanics.Player.Models;
	using GTANetworkAPI;

	public class DisplayAdmins : Script
	{
		private const string DisplayAdminsUsage = "Использование: /admins";

		[Command("admins", DisplayAdminsUsage, Alias = "a", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void OnDisplayAdmins(CPlayer admin)
		{
			// TODO
			// admin.SendChatMessage($"Админы онлайн: {AdminsCache.GetAdminNames()}");
		}
	}
}
