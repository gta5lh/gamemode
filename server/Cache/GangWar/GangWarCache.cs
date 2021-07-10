using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
using Gamemode.Models.Player;
using GTANetworkAPI;
using Quartz;
using Rpc.GangWar;

namespace Gamemode.Cache.GangWar
{
	public static class GangWarCache
	{
		public static DateTime StartTime { get; set; }
		public static DateTime FinishTime { get; set; }
		public static ColShape ColShape;
		public static Rpc.GangWar.GangWar? GangWar { get; set; }
		public static List<CustomPlayer> PlayersInZone { get; set; }
		private static ConcurrentDictionary<long, long> KillsByGangID { get; set; }
		private static bool isInProgress { get; set; }
		private static bool isFinishing { get; set; }


		public static void InitGangWarCache(Rpc.GangWar.GangWar _gangWar)
		{
			GangWar = _gangWar;
			KillsByGangID = new ConcurrentDictionary<long, long>();
			KillsByGangID.TryAdd(GangUtil.NpcIdBloods, 0);
			KillsByGangID.TryAdd(GangUtil.NpcIdBallas, 0);
			KillsByGangID.TryAdd(GangUtil.NpcIdTheFamilies, 0);
			KillsByGangID.TryAdd(GangUtil.NpcIdVagos, 0);
			KillsByGangID.TryAdd(GangUtil.NpcIdMarabunta, 0);
			isInProgress = false;
			isFinishing = false;
			PlayersInZone = new List<CustomPlayer>();
		}

		public static void ResetGangWarCache()
		{
			GangWar = null;
			KillsByGangID.Clear();
			isInProgress = false;
			isFinishing = false;
			PlayersInZone = new List<CustomPlayer>();
		}

		public static void SetAsInProgress()
		{
			isInProgress = true;
		}

		public static void SetAsFinishing()
		{
			isFinishing = true;
		}

		public static bool IsInited()
		{
			return GangWar != null;
		}

		public static bool IsInProgress()
		{
			return isInProgress;
		}

		public static bool IsFinishing()
		{
			return isFinishing;
		}

		public static void AddKill(long gangID, long amount)
		{
			KillsByGangID.AddOrUpdate(gangID, 1, (id, kills) => ((long)(kills + amount)));
		}

		public static GangWarStats GetGangWarStats()
		{
			long bloodsKills;
			long ballasKills;
			long theFamiliesKills;
			long vagosKills;
			long marabuntaKills;

			KillsByGangID.TryGetValue(GangUtil.NpcIdBloods, out bloodsKills);
			KillsByGangID.TryGetValue(GangUtil.NpcIdBallas, out ballasKills);
			KillsByGangID.TryGetValue(GangUtil.NpcIdTheFamilies, out theFamiliesKills);
			KillsByGangID.TryGetValue(GangUtil.NpcIdVagos, out vagosKills);
			KillsByGangID.TryGetValue(GangUtil.NpcIdMarabunta, out marabuntaKills);

			GangWarStats gangWarStats = new GangWarStats(ballasKills, bloodsKills, marabuntaKills, theFamiliesKills, vagosKills);

			return gangWarStats;
		}

		public static bool IsZeroKills()
		{
			lock (KillsByGangID)
			{
				short result = 0;

				foreach (short killsNumber in KillsByGangID.Values)
				{
					if (killsNumber <= 0) continue;

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

				if (kills[0] == kills[1]) return null;

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

		public static double RemainingMs()
		{
			StartTime = DateTime.UtcNow;
			return (FinishTime - StartTime).TotalMilliseconds;
		}
	}
}
