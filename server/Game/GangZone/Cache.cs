// <copyright file="Cache.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.GangZone
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Gamemode.Game.Gang;
	using Gamemode.Infrastructure;
	using GamemodeCommon.Models.Gang;

	public static class Cache
	{
		public static List<Zone> Zones { get; set; }

		public static async Task Init()
		{
			Zones = await LoadZones();
		}

		public static async Task<List<Zone>> LoadZones()
		{
			List<Zone> zones = new();

			Rpc.Zone.AllResponse allResponse = await RpcClients.ZoneService.AllAsync(new Rpc.Zone.AllRequest());
			foreach (Rpc.Zone.Zone zone in allResponse.Zones)
			{
				zones.Add(new Zone(zone.ID, zone.X, zone.Y, zone.FractionID, zone.Battleworthy, zone.Neutral ? (byte)55 : Util.BlipColorByGangId[zone.FractionID]));
			}

			return zones;
		}

		public static void MarkAsWarInProgress(long zoneID)
		{
			for (int i = 0; i < Zones.Count; i++)
			{
				if (Zones[i].Id == zoneID)
				{
					Zones[i].IsWarInProgress = true;
					return;
				}
			}
		}

		public static void MarkAsWarFinished(long zoneID, long winnerFractionID)
		{
			for (int i = 0; i < Zones.Count; i++)
			{
				if (Zones[i].Id == zoneID)
				{
					Zones[i].IsWarInProgress = false;
					Zones[i].BlipColor = Util.BlipColorByGangId[winnerFractionID];
					Zones[i].FractionId = winnerFractionID;
					return;
				}
			}
		}
	}
}
