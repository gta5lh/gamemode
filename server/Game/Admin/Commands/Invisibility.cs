// <copyright file="Invisibility.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Admin.Commands
{
	using Gamemode.Game.Admin.Models;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class Invisibility : BaseHandler
	{
		private const string InvisibilityUsage = "Использование: /invisibility. Пример: /i";

		[AdminMiddleware(AdminRank.Junior)]
		[Command("invisibility", InvisibilityUsage, Alias = "i", GreedyArg = true, Hide = true)]
		public static void OnInvisibility(CPlayer admin)
		{
			if (admin.Noclip || admin.Spectating)
			{
				admin.SendChatMessage("Изменить невидимку нельзя с включенным noclip или spectate");
				return;
			}

			admin.Invisible = !admin.Invisible;
			NAPI.ClientEvent.TriggerClientEvent(admin, "SetInvisibility");
		}
	}
}
