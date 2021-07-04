// <copyright file="Gang.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
	using Gamemode.Models.Npc;
	using Gamemode.Models.Spawn;
	using GTANetworkAPI;

	public abstract class Gang
	{
		public string Name;

		public Color GangColor;

		public byte GangBlipColor;

		public Marker ItemMarker;

		public Marker CarMarker;

		public Spawn PlayerSpawn;

		public Npc Npc;

		public void Create()
		{
			this.CarMarker.Create();
			// this.ItemMarker.Create();
			this.Npc.Create();
			NAPI.Blip.CreateBlip(543, this.PlayerSpawn.Position, 1, this.GangBlipColor, this.Name, 255, 0, true);
		}
	}
}
