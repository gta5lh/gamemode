using System.Collections.Generic;
using GTANetworkAPI;

namespace Gamemode.Models.Spawn
{
    public static class PlayerSpawns
    {
        public static readonly Spawn[] SpawnPositions = new Spawn[]
        {
            new Spawn(new Vector3(-1037.7, -2737.5, 20.17), -29.5f), // аэропорт
            new Spawn(new Vector3(432.9, -646, 28.7),  94), // автобусная станция
            new Spawn(new Vector3(-427.3, 1117.1, 326.76), -16), // обсерватория
        };

        public static readonly Dictionary<string, Spawn> VehicleSpawnByNpcName = new Dictionary<string, Spawn>()
        {
            { "michael", new Spawn(new Vector3(-390.25, 1189.7, 325.15), 101)},
            { "franklin", new Spawn(new Vector3(415.8, -643.9, 27.8), 66)},
            { "trevor", new Spawn(new Vector3(-1033, -2730, 19.62), -120.16f)},
        };
    }
}
