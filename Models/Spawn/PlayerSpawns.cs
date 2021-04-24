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
    }
}
