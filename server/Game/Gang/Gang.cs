// <copyright file="Gang.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Gang
{
	using Gamemode.Game.Npc;
	using Gamemode.Game.Spawn;
	using GTANetworkAPI;

	public abstract class Gang
	{
		public string Name { get; set; }

		public Color GangColor { get; set; }

		public byte GangBlipColor { get; set; }

		public Game.Marker.Marker ItemMarker { get; set; }

		public Game.Marker.Marker CarMarker { get; set; }

		public Spawn PlayerSpawn { get; set; }

		public Npc Npc { get; set; }

		public void Create()
		{
			this.CarMarker.Create();
			this.ItemMarker.Create();
			this.Npc.Create();
			NAPI.Blip.CreateBlip(543, this.PlayerSpawn.Position, 1, this.GangBlipColor, this.Name, 255, 0, true);
		}
	}
}
