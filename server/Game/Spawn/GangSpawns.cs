// <copyright file="GangSpawns.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Spawn
{
	using System.Collections.Generic;
	using System.Collections.Immutable;
	using Gamemode.Game.Gang;

	public static class GangSpawns
	{
		public static readonly ImmutableDictionary<long, Spawn> Spawns = new Dictionary<long, Spawn>()
		{
			{ 1, Bloods.Spawn },
			{ 2, Ballas.Spawn },
			{ 3, TheFamilies.Spawn },
			{ 4, Vagos.Spawn },
			{ 5, Marabunta.Spawn },
		}.ToImmutableDictionary();

		public static readonly ImmutableDictionary<string, Spawn> SpawnByGangName = new Dictionary<string, Spawn>()
		{
			{ "bloods", Bloods.Spawn },
			{ "ballas", Ballas.Spawn },
			{ "the_families", TheFamilies.Spawn },
			{ "vagos", Vagos.Spawn },
			{ "marabunta", Marabunta.Spawn },
		}.ToImmutableDictionary();
	}
}
