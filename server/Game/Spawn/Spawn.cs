// <copyright file="Spawn.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Spawn
{
	using GTANetworkAPI;

	public class Spawn
	{
		public Spawn(Vector3 position, float heading)
		{
			this.Position = position;
			this.Heading = heading;
		}

		public Vector3 Position { get; }

		public float Heading { get; }
	}
}
