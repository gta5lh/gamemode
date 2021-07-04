// <copyright file="Ballas.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
	using Gamemode.Colshape;
	using Gamemode.Models.Npc;
	using Gamemode.Models.Spawn;
	using GTANetworkAPI;

	public class Ballas : Gang
	{
		public static readonly byte BlipColor = 50;
		public static readonly Spawn CarSpawn = new Spawn(new Vector3(88.7, -1967.8, 20.75), -39.1f);
		public static readonly Spawn CarSelection = new Spawn(new Vector3(102.55f, -1948.45, 20.19f), 22.8f);
		public static readonly Spawn Spawn = new Spawn(new Vector3(89.6, -1952.5, 20.75), -37.86f);
		public static readonly Color Color = new Color(98, 18, 118);
		public static readonly int ColorClientSide = 145;

		public Ballas()
		{
			this.Name = "Ballas";
			this.GangColor = Color;
			this.PlayerSpawn = Spawn;
			this.CarMarker = new Marker(new Vector3(84.7, -1972.8, 20.84), this.GangColor, (MarkerType)36, "Автомобиль", new CarSelectionEvent(GangUtil.NpcIdBallas, CarSelection, ColorClientSide));
			this.ItemMarker = new Marker(new Vector3(102.9, -1959.3, 20.8), this.GangColor, (MarkerType)41, "Снаряжение", new ItemSelectionEvent(GangUtil.NpcIdBallas));
			this.Npc = new Npc(new Vector3(86.7, -1950, 20.8), -54, "Старший", PedHash.BallaOrig01GMY, new Colshape.GangNpcEvent(NpcUtil.NpcNameBallas, GangUtil.NpcIdBallas));
			this.GangBlipColor = BlipColor;
		}
	}
}
