using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
using GTANetworkAPI;
using Quartz;

namespace Gamemode.Cache.GangWar
{
	public static class GangWarCache
	{
		public static ColShape ColShape;
		public static ApiClient.Models.GangWar? GangWar { get; set; }
		private static ConcurrentDictionary<byte, short> KillsByGangID { get; set; }
		private static bool isInProgress { get; set; }
		private static bool isFinishing { get; set; }

		public static void InitGangWarCache(ApiClient.Models.GangWar _gangWar)
		{
			GangWar = _gangWar;
			KillsByGangID = new ConcurrentDictionary<byte, short>();
			KillsByGangID.TryAdd(GangUtil.NpcIdBloods, 0);
			KillsByGangID.TryAdd(GangUtil.NpcIdBallas, 0);
			KillsByGangID.TryAdd(GangUtil.NpcIdTheFamilies, 0);
			KillsByGangID.TryAdd(GangUtil.NpcIdVagos, 0);
			KillsByGangID.TryAdd(GangUtil.NpcIdMarabunta, 0);
			isInProgress = false;
			isFinishing = false;
		}

		public static void ResetGangWarCache()
		{
			GangWar = null;
			KillsByGangID.Clear();
			isInProgress = false;
			isFinishing = false;
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

		public static void AddKill(byte gangID)
		{
			KillsByGangID.AddOrUpdate(gangID, 1, (id, kills) => ++kills);
		}

		public static string GetKillsMessage()
		{
			short bloodsKills;
			short ballasKills;
			short theFamiliesKills;
			short vagosKills;
			short marabuntaKills;

			KillsByGangID.TryGetValue(GangUtil.NpcIdBloods, out bloodsKills);
			KillsByGangID.TryGetValue(GangUtil.NpcIdBallas, out ballasKills);
			KillsByGangID.TryGetValue(GangUtil.NpcIdTheFamilies, out theFamiliesKills);
			KillsByGangID.TryGetValue(GangUtil.NpcIdVagos, out vagosKills);
			KillsByGangID.TryGetValue(GangUtil.NpcIdMarabunta, out marabuntaKills);

			return $"Bloods={bloodsKills} Ballas={ballasKills} TheFamilies={theFamiliesKills} Vagos={vagosKills} Marabunta={marabuntaKills}";
		}

		public static bool IsZeroKills()
		{
			lock (KillsByGangID)
			{
				short result = 0;

				foreach (short killsNumber in KillsByGangID.Values)
				{
					result += killsNumber;
				}

				return result <= 0;
			}
		}

		public static byte? GetWinner()
		{
			lock (KillsByGangID)
			{
				short[] kills = KillsByGangID.Values.ToArray();
				Array.Sort(kills);
				Array.Reverse(kills);

				if (kills[0] == kills[1]) return null;

				foreach (KeyValuePair<byte, short> keyValuePair in KillsByGangID)
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

				foreach (KeyValuePair<byte, short> keyValuePair in KillsByGangID)
				{
					statistics.Add(new GangWarStatistics(keyValuePair.Key, keyValuePair.Value));
				}

				return statistics;
			}
		}
	}
}
