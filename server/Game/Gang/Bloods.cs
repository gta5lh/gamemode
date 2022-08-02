// <copyright file="Bloods.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Gang
{
	using Gamemode.Game.Colshape;
	using Gamemode.Game.Npc;
	using Gamemode.Game.Spawn;
	using GamemodeCommon.Models;
	using GTANetworkAPI;

	public class Bloods : Gang
	{
		public static readonly byte BlipColor = 75;
		public static readonly Spawn CarSpawn = new(new Vector3(489, -1313, 29.26), -74.2f);
		public static readonly Spawn CarSelection = new(new Vector3(492.39f, -1333.11, 28.76), -136.08f);
		public static readonly Spawn Spawn = new(new Vector3(490, -1335, 29), -45f);
		public static readonly Color Color = new(222, 15, 24);
		public static readonly int ColorClientSide = 44;

		public Bloods()
		{
			this.Name = "Bloods";
			this.GangColor = Color;
			this.PlayerSpawn = Spawn;
			this.CarMarker = new Game.Marker.Marker(new Vector3(487, -1314, 29.2), this.GangColor, (MarkerType)36, "Автомобиль", new CarSelectionEvent(Util.NpcIdBloods, CarSelection, ColorClientSide));
			this.ItemMarker = new Game.Marker.Marker(new Vector3(502, -1339, 29.26), this.GangColor, (MarkerType)41, "Снаряжение", new ItemSelectionEvent(Util.NpcIdBloods));
			this.Npc = new Npc(new Vector3(499, -1326.3, 29.33), 87.4f, "Старший", PedHash.PartyTarget, new GangNpcEvent(NpcNames.Bloods, Util.NpcIdBloods));
			this.GangBlipColor = BlipColor;
		}
	}
}
