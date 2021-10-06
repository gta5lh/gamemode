using System;
using Gamemode.Models.Gangs;
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
			int health = player.Health;

			if (player.Fraction != null)
			{
				Spawn spawn = GangSpawns.Spawns[player.Fraction.Value];
				player.Position = spawn.Position;
				player.Heading = spawn.Heading;
				return;
			}

			int spawnPosition = random.Next(0, 3);
			player.Position = PlayerSpawns.SpawnPositions[spawnPosition].Position;
			player.Heading = PlayerSpawns.SpawnPositions[spawnPosition].Heading;
			NAPI.ClientEvent.TriggerClientEvent(player, "DisplayNoviceNotification");
		}
	}
}
