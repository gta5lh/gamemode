// <copyright file="CarSelectionEvent.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

using Gamemode.Cache.GangWar;
using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Colshape
{
	public class GangWarEvent : IColShapeEventHandler
	{
		public void OnEntityEnterColShape(ColShape shape, Player player)
		{
			CustomPlayer customPlayer = (CustomPlayer)player;
			if (customPlayer.Fraction == null)
			{
				return;
			}

			customPlayer.IsInWarZone = true;
			player.TriggerEvent("SetZoneState", true, "red");
			GangWarCache.PlayersInZone.Add(customPlayer);
		}

		public void OnEntityExitColShape(ColShape shape, Player player)
		{
			CustomPlayer customPlayer = (CustomPlayer)player;
			if (customPlayer.Fraction == null)
			{
				return;
			}

			customPlayer.IsInWarZone = false;
			player.TriggerEvent("SetZoneState", false, "red");
			GangWarCache.PlayersInZone.Remove(customPlayer);
		}
	}
}
