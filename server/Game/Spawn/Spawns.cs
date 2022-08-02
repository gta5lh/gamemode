// <copyright file="Spawns.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Spawn
{
	using Gamemode.Game.Gang;

	public static class Spawns
	{
		public static readonly Spawn[] All = new Spawn[]
		{
			Bloods.Spawn,
			Ballas.Spawn,
			TheFamilies.Spawn,
			Vagos.Spawn,
			Marabunta.Spawn,
			PlayerSpawns.SpawnPositions[0],
			PlayerSpawns.SpawnPositions[1],
			PlayerSpawns.SpawnPositions[2],
		};
	}
}
