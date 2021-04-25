using GTANetworkAPI;

namespace Gamemode.Models.Npc
{
    public static class SpawnNpcs
    {
        private static readonly Npc[] spawnNpcs = new Npc[]
        {
            new Npc(new Vector3(-1032.5, -2734.5, 20.17), 133f, "Тревор", PedHash.Trevor, new Colshape.MichaelEvent()), // аэропорт
            new Npc(new Vector3(412.6, -633.9, 28.5), -178f, "Франклин", PedHash.Franklin, new Colshape.MichaelEvent()), // автобусная станция
            new Npc(new Vector3(-422.48, 1126, 325.9), 145f, "Майкл", PedHash.Michael, new Colshape.MichaelEvent()), // обсерватория
        };

        public static void CreateSpawnNpcs()
        {
            foreach (Npc npc in spawnNpcs)
            {
                npc.Create();
            }
        }
    }
}
