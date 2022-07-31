// <copyright file="GangSpawns.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Gang
{
	using System.Collections.Generic;
	using Gamemode.Game.Spawn;

	public static class GangSpawns
	{
		public static readonly Dictionary<long, Spawn> Spawns = new Dictionary<long, Spawn>()
		{
			{ 1, Bloods.Spawn },
			{ 2, Ballas.Spawn },
			{ 3, TheFamilies.Spawn },
			{ 4, Vagos.Spawn },
			{ 5, Marabunta.Spawn },
		};

		public static readonly Dictionary<string, Spawn> SpawnByGangName = new Dictionary<string, Spawn>()
		{
			{ "bloods", Bloods.Spawn },
			{ "ballas", Ballas.Spawn },
			{ "the_families", TheFamilies.Spawn },
			{ "vagos", Vagos.Spawn },
			{ "marabunta", Marabunta.Spawn },
		};
	}
}
