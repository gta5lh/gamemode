// <copyright file="Speed.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace GamemodeClient.Game.Vehicle
{
	using System;
	using RAGE.Elements;

	public static class Speed
	{
		public static int GetPlayerRealSpeed(Player player)
		{
			return Convert.ToInt32(player.GetSpeed() * 3.6);
		}
	}
}
