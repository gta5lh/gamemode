// <copyright file="GangZone.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.GangZone.Events
{
	using Gamemode.Game.GangZone;
	using GTANetworkAPI;

	public class GangZone : Script
	{
		[ServerEvent(Event.PlayerConnected)]
		public void OnPlayerConnected(Player player)
		{
			NAPI.ClientEvent.TriggerClientEvent(player, "RenderGangZones", Cache.Zones);
		}
	}
}
