using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using GamemodeCommon.Models.Gang;
using Gamemode.Infrastructure;
using GTANetworkAPI;
using Quartz;

namespace Gamemode.Cache.GangZone
{
	public static class GangZoneCache
	{
		public static List<Zone> Zones { get; set; }

		public static async void InitGangZoneCache()
		{
			Zones = await LoadZones();
		}

		public static async Task<List<Zone>> LoadZones()
		{
			Rpc.Zone.AllResponse allResponse;
			List<Zone> zones = new List<Zone>();

			try
			{
				allResponse = await RpcClients.ZoneService.AllAsync(new Rpc.Zone.AllRequest());
			}
			catch (Exception)
			{
				return null;
			}

			foreach (Rpc.Zone.Zone zone in allResponse.Zones)
			{
				zones.Add(new Zone(zone.ID, zone.X, zone.Y, zone.FractionID, zone.Battleworthy, GangUtil.BlipColorByGangId[zone.FractionID]));
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
					Zones[i].BlipColor = GangUtil.BlipColorByGangId[winnerFractionID];
					Zones[i].FractionId = winnerFractionID;
					return;
				}
			}
		}
	}
}
