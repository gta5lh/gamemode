// <copyright file="GangWarEvent.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Colshape
{
	using Gamemode.Game.GangWar;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class GangWarEvent : IColShapeEventHandler
	{
		public void OnEntityEnterColShape(ColShape shape, Player player)
		{
			CPlayer cPlayer = (CPlayer)player;
			if (cPlayer.Fraction == null)
			{
				return;
			}

			cPlayer.IsInWarZone = true;
			player.TriggerEvent("SetZoneState", true, "red");
			Cache.PlayersInZone.Add(cPlayer);
		}

		public void OnEntityExitColShape(ColShape shape, Player player)
		{
			CPlayer cPlayer = (CPlayer)player;
			if (cPlayer.Fraction == null)
			{
				return;
			}

			cPlayer.IsInWarZone = false;
			player.TriggerEvent("SetZoneState", false, "red");
			Cache.PlayersInZone.Remove(cPlayer);
		}
	}
}
