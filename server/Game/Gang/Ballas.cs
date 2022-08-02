// <copyright file="Ballas.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Gang
{
	using Gamemode.Game.Colshape;
	using Gamemode.Game.Npc;
	using Gamemode.Game.Spawn;
	using GamemodeCommon.Models;
	using GTANetworkAPI;

	public class Ballas : Gang
	{
		public static readonly byte BlipColor = 50;
		public static readonly Spawn CarSpawn = new(new Vector3(88.7, -1967.8, 20.75), -39.1f);
		public static readonly Spawn CarSelection = new(new Vector3(102.55f, -1948.45, 20.19f), 22.8f);
		public static readonly Spawn Spawn = new(new Vector3(89.6, -1952.5, 20.75), -37.86f);
		public static readonly Color Color = new(98, 18, 118);
		public static readonly int ColorClientSide = 145;

		public Ballas()
		{
			this.Name = "Ballas";
			this.GangColor = Color;
			this.PlayerSpawn = Spawn;
			this.CarMarker = new Game.Marker.Marker(new Vector3(84.7, -1972.8, 20.84), this.GangColor, (MarkerType)36, "Автомобиль", new CarSelectionEvent(Util.NpcIdBallas, CarSelection, ColorClientSide));
			this.ItemMarker = new Game.Marker.Marker(new Vector3(102.9, -1959.3, 20.8), this.GangColor, (MarkerType)41, "Снаряжение", new ItemSelectionEvent(Util.NpcIdBallas));
			this.Npc = new Npc(new Vector3(86.7, -1950, 20.8), -54, "Старший", PedHash.BallaOrig01GMY, new GangNpcEvent(NpcNames.Ballas, Util.NpcIdBallas));
			this.GangBlipColor = BlipColor;
		}
	}
}
