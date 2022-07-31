// <copyright file="GangNpcEvent.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Colshape
{
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class GangNpcEvent : IColShapeEventHandler
	{
		private readonly string npcName;
		private readonly byte fractionId;

		public GangNpcEvent(string npcName, byte fractionId)
		{
			this.npcName = npcName;
			this.fractionId = fractionId;
		}

		public void OnEntityEnterColShape(ColShape shape, Player player)
		{
			if (player.IsInVehicle)
			{
				return;
			}

			CPlayer cPlayer = (CPlayer)player;
			if (cPlayer.Fraction != null && cPlayer.Fraction.Value != this.fractionId)
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
