using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using GTANetworkAPI;
using Quartz;

namespace Gamemode.Cache.GangWar
{
    public static class GangWarCache
    {
        public static ApiClient.Models.GangWar? GangWar { get; set; }
        private static ConcurrentDictionary<byte, int> KillsByGangID { get; set; }
        private static bool isInProgress { get; set; }

        public static void InitGangWarCache(ApiClient.Models.GangWar _gangWar)
        {
            GangWar = _gangWar;
            KillsByGangID = new ConcurrentDictionary<byte, int>();
            KillsByGangID.TryAdd(GangUtil.NpcIdBloods, 0);
            KillsByGangID.TryAdd(GangUtil.NpcIdBallas, 0);
            KillsByGangID.TryAdd(GangUtil.NpcIdTheFamilies, 0);
            KillsByGangID.TryAdd(GangUtil.NpcIdVagos, 0);
            KillsByGangID.TryAdd(GangUtil.NpcIdMarabunta, 0);
            isInProgress = false;
        }

        public static void SetAsInProgress()
        {
            isInProgress = true;
        }

        public static bool IsInited()
        {
            return GangWar != null;
        }

        public static bool IsInProgress()
        {
            return isInProgress;
        }

        public static void AddKill(byte gangID)
        {
            KillsByGangID.AddOrUpdate(gangID, 1, (id, kills) => kills + 1);
        }

        public static string GetKillsMessage()
        {
            int bloodsKills;
            int ballasKills;
            int theFamiliesKills;
            int vagosKills;
            int marabuntaKills;

            KillsByGangID.TryGetValue(GangUtil.NpcIdBloods, out bloodsKills);
            KillsByGangID.TryGetValue(GangUtil.NpcIdBallas, out ballasKills);
            KillsByGangID.TryGetValue(GangUtil.NpcIdTheFamilies, out theFamiliesKills);
            KillsByGangID.TryGetValue(GangUtil.NpcIdVagos, out vagosKills);
            KillsByGangID.TryGetValue(GangUtil.NpcIdMarabunta, out marabuntaKills);

            return $"Bloods={bloodsKills} Ballas={ballasKills} TheFamilies={theFamiliesKills} Vagos={vagosKills} Marabunta={marabuntaKills}";
        }
    }
}
