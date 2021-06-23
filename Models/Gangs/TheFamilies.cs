// <copyright file="TheFamilies.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
	using Gamemode.Colshape;
	using Gamemode.Models.Npc;
	using Gamemode.Models.Spawn;
	using GTANetworkAPI;

	public class TheFamilies : Gang
	{
		public static readonly byte BlipColor = 2;
		public static readonly Spawn CarSpawn = new Spawn(new Vector3(-24, -1436, 30.65), -179f);
		public static readonly Spawn CarSelection = new Spawn(new Vector3(-22.65f, -1457.9f, 30.05f), -118.6f);
		public static readonly Spawn Spawn = new Spawn(new Vector3(-14, -1446, 30.65), 179);
		public static readonly Color Color = new Color(21, 92, 45);
		public static readonly int ColorClientSide = 53;

		public TheFamilies()
		{
			this.Name = "The Families";
			this.GangColor = Color;
			this.PlayerSpawn = Spawn;
			this.CarMarker = new Marker(new Vector3(-25, -1433, 30.65), this.GangColor, (MarkerType)36, "Автомобиль", new CarSelectionEvent(GangUtil.NpcIdTheFamilies, CarSelection, ColorClientSide));
			this.ItemMarker = new Marker(new Vector3(-10, -1445, 30.75), this.GangColor, (MarkerType)41, "Снаряжение", new ItemSelectionEvent(GangUtil.NpcIdTheFamilies));
			this.Npc = new Npc(new Vector3(-18, -1448, 30.65), -48, "Старший", PedHash.Stretch, new Colshape.GangNpcEvent(NpcUtil.NpcNameTheFamilies, GangUtil.NpcIdTheFamilies));
			this.GangBlipColor = BlipColor;
		}
	}
}
