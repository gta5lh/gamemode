// <copyright file="Marabunta.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Gang
{
	using Gamemode.Game.Colshape;
	using Gamemode.Game.Npc;
	using Gamemode.Game.Spawn;
	using GamemodeCommon.Models;
	using GTANetworkAPI;

	public class Marabunta : Gang
	{
		public static readonly byte BlipColor = 67;
		public static readonly Spawn CarSpawn = new(new Vector3(1374.1, -1523.4, 57), 174.37f);
		public static readonly Spawn Spawn = new(new Vector3(1378.1, -1518.1, 57.79), 147.75f);
		public static readonly Color Color = new(11, 156, 241);
		public static readonly int ColorClientSide = 70;

		public Marabunta()
		{
			this.Name = "Marabunta";
			this.GangColor = Color;
			this.PlayerSpawn = Spawn;
			this.CarMarker = new Game.Marker.Marker(new Vector3(1371.9, -1519.2, 57.52), this.GangColor, (MarkerType)36, "Автомобиль", new CarSelectionEvent(Util.NpcIdMarabunta, CarSpawn, ColorClientSide));
			this.ItemMarker = new Game.Marker.Marker(new Vector3(1384.2, -1521.7, 57.53), this.GangColor, (MarkerType)41, "Снаряжение", new ItemSelectionEvent(Util.NpcIdMarabunta));
			this.Npc = new Npc(new Vector3(1367, -1527, 56.7), -89.8f, "Старший", PedHash.SalvaGoon01GMY, new GangNpcEvent(NpcNames.Marabunta, Util.NpcIdMarabunta));
			this.GangBlipColor = BlipColor;
		}
	}
}
