// <copyright file="CarSelectionEvent.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Colshape
{
	public class GangNpcEvent : IColShapeEventHandler
	{
		private string NpcName;
		private byte FractionId;

		public GangNpcEvent(string npcName, byte fractionId)
		{
			this.NpcName = npcName;
			this.FractionId = fractionId;
		}

		public void OnEntityEnterColShape(ColShape shape, Player player)
		{
			string state = this.State(player);
			if (state == string.Empty)
			{
				return;
			}

			NAPI.ClientEvent.TriggerClientEvent(player, "DisplayPressE", true, this.NpcName, state);
		}

		public void OnEntityExitColShape(ColShape shape, Player player)
		{
			NAPI.ClientEvent.TriggerClientEvent(player, "DisplayPressE", false, this.NpcName);
		}

		private string State(Player player)
		{
			CustomPlayer customPlayer = (CustomPlayer)player;
			if (customPlayer.Fraction == null)
			{
				return "join";
			}

			if (customPlayer.IsInVehicle || customPlayer.Fraction.Value != this.FractionId)
			{
				return string.Empty;
			}

			return "leave";
		}
	}
}
