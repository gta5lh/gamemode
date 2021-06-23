// <copyright file="Bloods.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
	using Gamemode.Colshape;
	using Gamemode.Models.Npc;
	using Gamemode.Models.Spawn;
	using GTANetworkAPI;

	public class Bloods : Gang
	{
		public static readonly byte BlipColor = 75;
		public static readonly Spawn CarSpawn = new Spawn(new Vector3(489, -1313, 29.26), -74.2f);
		public static readonly Spawn CarSelection = new Spawn(new Vector3(492.39f, -1333.11, 28.76), -136.08f);
		public static readonly Spawn Spawn = new Spawn(new Vector3(490, -1335, 29), -45f);
		public static readonly Color Color = new Color(222, 15, 24);
		public static readonly int ColorClientSide = 44;

		public Bloods()
		{
			this.Name = "Bloods";
			this.GangColor = Color;
			this.PlayerSpawn = Spawn;
			this.CarMarker = new Marker(new Vector3(487, -1314, 29.2), this.GangColor, (MarkerType)36, "Автомобиль", new CarSelectionEvent(GangUtil.NpcIdBloods, CarSelection, ColorClientSide));
			this.ItemMarker = new Marker(new Vector3(502, -1339, 29.26), this.GangColor, (MarkerType)41, "Снаряжение", new ItemSelectionEvent(GangUtil.NpcIdBloods));
			this.Npc = new Npc(new Vector3(499, -1326.3, 29.33), 87.4f, "Старший", PedHash.PartyTarget, new Colshape.GangNpcEvent(NpcUtil.NpcNameBloods, GangUtil.NpcIdBloods));
			this.GangBlipColor = BlipColor;
		}
	}
}
