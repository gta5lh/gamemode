// <copyright file="ItemSelectionEvent.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Colshape
{
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class ItemSelectionEvent : IColShapeEventHandler
	{
		private readonly byte fractionId;

		public ItemSelectionEvent(byte fractionId)
		{
			this.fractionId = fractionId;
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

			NAPI.ClientEvent.TriggerClientEvent(player, "DisplayGangItemSelectionMenu", true);
		}

		public void OnEntityExitColShape(ColShape shape, Player player)
		{
			NAPI.ClientEvent.TriggerClientEvent(player, "DisplayGangItemSelectionMenu", false);
		}
	}
}
