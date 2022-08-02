// <copyright file="SpawnNpcEvent.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Colshape
{
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class SpawnNpcEvent : IColShapeEventHandler
	{
		private readonly string npcName;

		public SpawnNpcEvent(string npcName)
		{
			this.npcName = npcName;
		}

		public void OnEntityEnterColShape(ColShape shape, Player player)
		{
			CPlayer cPlayer = (CPlayer)player;
			if (cPlayer.IsInVehicle || cPlayer.Fraction != null)
			{
				return;
			}

			NAPI.ClientEvent.TriggerClientEvent(player, "DisplayPressE", true, this.npcName);
		}

		public void OnEntityExitColShape(ColShape shape, Player player)
		{
			NAPI.ClientEvent.TriggerClientEvent(player, "DisplayPressE", false, this.npcName);
		}
	}
}
