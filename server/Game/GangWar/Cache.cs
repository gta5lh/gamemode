// <copyright file="Cache.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.GangWar
{
	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.Linq;
	using Gamemode.Game.Gang;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;
	using Rpc.GangWar;

	public static class Cache
	{
		public static DateTime StartTime { get; set; }

		public static DateTime FinishTime { get; set; }

		public static ColShape ColShape;

		public static Rpc.GangWar.GangWar? GangWar { get; set; }

		public static List<CPlayer> PlayersInZone { get; set; }

		private static ConcurrentDictionary<long, long> KillsByGangID { get; set; }

		private static bool IsInProgress1 { get; set; }

		private static bool IsFinishing1 { get; set; }

		public static void InitGangWarCache(Rpc.GangWar.GangWar gangWar)
		{
			GangWar = gangWar;
			KillsByGangID = new ConcurrentDictionary<long, long>();
			KillsByGangID.TryAdd(Util.NpcIdBloods, 0);
			KillsByGangID.TryAdd(Util.NpcIdBallas, 0);
			KillsByGangID.TryAdd(Util.NpcIdTheFamilies, 0);
			KillsByGangID.TryAdd(Util.NpcIdVagos, 0);
			KillsByGangID.TryAdd(Util.NpcIdMarabunta, 0);
			IsInProgress1 = false;
			IsFinishing1 = false;
			PlayersInZone = new List<CPlayer>();
		}

		public static void ResetGangWarCache()
		{
			GangWar = null;
			KillsByGangID.Clear();
			IsInProgress1 = false;
			IsFinishing1 = false;
			PlayersInZone = new List<CPlayer>();
		}

		public static void SetAsInProgress()
		{
			IsInProgress1 = true;
		}

		public static void SetAsFinishing()
		{
			IsFinishing1 = true;
		}

		public static bool IsInited()
		{
			return GangWar != null;
		}

		public static bool IsInProgress()
		{
			return IsInProgress1;
		}

		public static bool IsFinishing()
		{
			return IsFinishing1;
		}

		public static void AddKill(long gangID, long amount)
		{
			KillsByGangID.AddOrUpdate(gangID, 1, (id, kills) => (long)(kills + amount));
		}

		public static Stats GetGangWarStats()
		{
			long bloodsKills;
			long ballasKills;
			long theFamiliesKills;
			long vagosKills;
			long marabuntaKills;

			KillsByGangID.TryGetValue(Util.NpcIdBloods, out bloodsKills);
			KillsByGangID.TryGetValue(Util.NpcIdBallas, out ballasKills);
			KillsByGangID.TryGetValue(Util.NpcIdTheFamilies, out theFamiliesKills);
			KillsByGangID.TryGetValue(Util.NpcIdVagos, out vagosKills);
			KillsByGangID.TryGetValue(Util.NpcIdMarabunta, out marabuntaKills);

			Stats gangWarStats = new Stats(ballasKills, bloodsKills, marabuntaKills, theFamiliesKills, vagosKills);

			return gangWarStats;
		}

		public static bool IsZeroKills()
		{
			lock (KillsByGangID)
			{
				short result = 0;

				foreach (short killsNumber in KillsByGangID.Values.OfType<short>())
				{
					if (killsNumber <= 0)
					{
						continue;
					}

					result += killsNumber;
				}

				return result <= 0;
			}
		}

		public static long? GetWinner()
		{
			lock (KillsByGangID)
			{
				long[] kills = KillsByGangID.Values.ToArray();
				Array.Sort(kills);
				Array.Reverse(kills);

				if (kills[0] == kills[1])
				{
					return null;
				}

				foreach (KeyValuePair<long, long> keyValuePair in KillsByGangID)
				{
					if (keyValuePair.Value == kills[0])
					{
						return keyValuePair.Key;
					}
				}

				return null;
			}
		}

		public static ICollection<GangWarStatistics> GetStatistics()
		{
			lock (KillsByGangID)
			{
				List<GangWarStatistics> statistics = new List<GangWarStatistics>();

				foreach (KeyValuePair<long, long> keyValuePair in KillsByGangID)
				{
					GangWarStatistics gangWarStatistics = new GangWarStatistics();
					gangWarStatistics.FractionID = keyValuePair.Key;
					gangWarStatistics.KillsNumber = keyValuePair.Value;

					statistics.Add(gangWarStatistics);
				}

				return statistics;
			}
		}
	}
}
