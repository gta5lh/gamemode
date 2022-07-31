// <copyright file="Noclip.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Admin.Commands
{
	using Gamemode.Game.Admin.Models;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class Noclip : BaseHandler
	{
		[RemoteEvent("SetNoclip")]
		private static void OnSetNoclip(CPlayer admin, string request)
		{
			if (admin.AdminRank == 0)
			{
				return;
			}

			admin.Noclip = bool.Parse(request);
		}
	}
}
