// <copyright file="PlayerSpawns.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Spawn
{
	using System.Collections.Generic;
	using System.Collections.Immutable;
	using GTANetworkAPI;

	public static class PlayerSpawns
	{
		public static readonly Spawn[] SpawnPositions = new Spawn[]
		{
			new Spawn(new Vector3(-1037.7, -2737.5, 20.17), -29.5f), // аэропорт
			new Spawn(new Vector3(432.9, -646, 28.7),  94), // автобусная станция
			new Spawn(new Vector3(-427.3, 1117.1, 326.76), -16), // обсерватория
		};

		public static readonly ImmutableDictionary<string, Spawn> VehicleSpawnByNpcName = new Dictionary<string, Spawn>
		{
			{ "Майкл", new Spawn(new Vector3(-390.25, 1189.7, 325.15), 101) },
			{ "Франклин", new Spawn(new Vector3(416.26, -644.19, 27.975), -99.1f) },
			{ "Тревор", new Spawn(new Vector3(-1033, -2730, 19.62), -120.16f) },
		}.ToImmutableDictionary();
	}
}
