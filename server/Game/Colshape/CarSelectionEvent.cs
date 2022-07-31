// <copyright file="CarSelectionEvent.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Colshape
{
	using Gamemode.Game.Player.Models;
	using Gamemode.Game.Spawn;
	using GTANetworkAPI;

	public class CarSelectionEvent : IColShapeEventHandler
	{
		private readonly byte fractionId;
		private readonly Spawn vehicleSpawnPosition;
		private readonly int color;

		public CarSelectionEvent(byte fractionId, Spawn vehicleSpawnPosition, int color)
		{
			this.fractionId = fractionId;
			this.vehicleSpawnPosition = vehicleSpawnPosition;
			this.color = color;
		}

		public void OnEntityEnterColShape(ColShape shape, Player player)
		{
			CPlayer cPlayer = (CPlayer)player;
			if (cPlayer.Fraction != this.fractionId)
			{
				return;
			}

			if (cPlayer.IsInVehicle)
			{
				return;
			}

			NAPI.ClientEvent.TriggerClientEvent(player, "DisplayGangCarSelectionMenu", true, this.vehicleSpawnPosition, this.color, this.fractionId, cPlayer.FractionRank);
		}

		public void OnEntityExitColShape(ColShape shape, Player player)
		{
			NAPI.ClientEvent.TriggerClientEvent(player, "DisplayGangCarSelectionMenu", false);
		}
	}
}
