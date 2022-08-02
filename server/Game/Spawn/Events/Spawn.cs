// <copyright file="Spawn.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Spawn.Events
{
	using System;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class Spawn : Script
	{
		private readonly Random random = new Random();

		[ServerEvent(Event.PlayerSpawn)]
		private void OnPlayerSpawn(CPlayer player)
		{
			if (player.Fraction != null)
			{
				Game.Spawn.Spawn spawn = GangSpawns.Spawns[player.Fraction.Value];
				player.Position = spawn.Position;
				player.Heading = spawn.Heading;
				return;
			}

			int spawnPosition = this.random.Next(0, PlayerSpawns.SpawnPositions.Length);
			player.Position = PlayerSpawns.SpawnPositions[spawnPosition].Position;
			player.Heading = PlayerSpawns.SpawnPositions[spawnPosition].Heading;
		}
	}
}
