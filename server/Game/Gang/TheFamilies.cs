// <copyright file="TheFamilies.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Gang
{
	using Gamemode.Game.Colshape;
	using Gamemode.Game.Npc;
	using Gamemode.Game.Spawn;
	using GamemodeCommon.Models;
	using GTANetworkAPI;

	public class TheFamilies : Gang
	{
		public static readonly byte BlipColor = 2;
		public static readonly Spawn CarSpawn = new(new Vector3(-24, -1436, 30.65), -179f);
		public static readonly Spawn CarSelection = new(new Vector3(-22.65f, -1457.9f, 30.05f), -118.6f);
		public static readonly Spawn Spawn = new(new Vector3(-14, -1446, 30.65), 179);
		public static readonly Color Color = new(21, 92, 45);
		public static readonly int ColorClientSide = 53;

		public TheFamilies()
		{
			this.Name = "The Families";
			this.GangColor = Color;
			this.PlayerSpawn = Spawn;
			this.CarMarker = new Game.Marker.Marker(new Vector3(-25, -1433, 30.65), this.GangColor, (MarkerType)36, "Автомобиль", new CarSelectionEvent(Util.NpcIdTheFamilies, CarSelection, ColorClientSide));
			this.ItemMarker = new Game.Marker.Marker(new Vector3(-10, -1445, 30.75), this.GangColor, (MarkerType)41, "Снаряжение", new ItemSelectionEvent(Util.NpcIdTheFamilies));
			this.Npc = new Npc(new Vector3(-18, -1448, 30.65), -48, "Старший", PedHash.Stretch, new GangNpcEvent(NpcNames.TheFamilies, Util.NpcIdTheFamilies));
			this.GangBlipColor = BlipColor;
		}
	}
}
