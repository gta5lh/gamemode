using System;
using Gamemode.Models.Player;
using Gamemode.Models.Spawn;
using GTANetworkAPI;

namespace Gamemode.Controllers
{
    public class SpawnController : Script
    {
        private readonly Random random = new Random();

        [ServerEvent(Event.PlayerSpawn)]
        private void OnPlayerSpawn(CustomPlayer player)
        {
            int spawnPosition = random.Next(0, 3);
            player.Position = PlayerSpawns.SpawnPositions[0].Position;
            player.Heading = PlayerSpawns.SpawnPositions[0].Heading;
        }
    }
}
