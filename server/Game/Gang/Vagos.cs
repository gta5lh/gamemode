// <copyright file="Vagos.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Gang
{
	using Gamemode.Game.Colshape;
	using Gamemode.Game.Npc;
	using Gamemode.Game.Spawn;
	using GamemodeCommon.Models;
	using GTANetworkAPI;

	public class Vagos : Gang
	{
		public static readonly byte BlipColor = 46;
		public static readonly Spawn CarSpawn = new(new Vector3(330, -2042, 20.85), -42.1f);
		public static readonly Spawn CarSelection = new(new Vector3(319.44f, -2027.07f, 20.13f), -45.75f);
		public static readonly Spawn Spawn = new(new Vector3(336, -2054, 20.84), 3.16f);
		public static readonly Color Color = new(241, 204, 64);
		public static readonly int ColorClientSide = 126;

		public Vagos()
		{
			this.Name = "Vagos";
			this.GangColor = Color;
			this.PlayerSpawn = Spawn;
			this.CarMarker = new Game.Marker.Marker(new Vector3(330, -2042, 20.8), this.GangColor, (MarkerType)36, "Автомобиль", new CarSelectionEvent(Util.NpcIdVagos, CarSelection, ColorClientSide));
			this.ItemMarker = new Game.Marker.Marker(new Vector3(327, -2049, 20.84), this.GangColor, (MarkerType)41, "Снаряжение", new ItemSelectionEvent(Util.NpcIdVagos));
			this.Npc = new Npc(new Vector3(345.5, -2049, 21.6), 52.5f, "Старший", PedHash.Vagos01GFY, new GangNpcEvent(NpcNames.Vagos, Util.NpcIdVagos));
			this.GangBlipColor = BlipColor;
		}
	}
}
