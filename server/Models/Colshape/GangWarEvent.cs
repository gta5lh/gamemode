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
			customPlayer.SendChatMessage("Ты вошел в зону захвата территории");
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
			customPlayer.SendChatMessage("Ты вышел из зоны захвата территории");
			GangWarCache.PlayersInZone.Remove(customPlayer);
		}
	}
}
