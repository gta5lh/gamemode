using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
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
			List<Zone> zones;

			try
			{
				zones = await ApiClient.ApiClient.AllZones();
			}
			catch (Exception)
			{
				return null;
			}

			for (int i = 0; i < zones.Count; i++)
			{
				zones[i].BlipColor = GangUtil.BlipColorByGangId[zones[i].FractionId];
			}

			return zones;
		}

		public static void MarkAsWarInProgress(int zoneID)
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

		public static void MarkAsWarFinished(int zoneID, byte winnerFractionID)
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
