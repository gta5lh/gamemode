// <copyright file="Dimension.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Dimension
{
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class Dimension : Script
	{
		[RemoteProc("SetOwnDimension")]
		private static uint OnSetOwnDimension(CPlayer player)
		{
			player.Dimension = player.Id + 1U;
			return player.Dimension;
		}

		[RemoteEvent("SetServerDimension")]
		private static void OnSetServerDimension(CPlayer player)
		{
			player.Dimension = 0;
		}
	}
}
