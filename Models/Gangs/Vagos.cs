// <copyright file="Vagos.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using Gamemode.Colshape;
    using Gamemode.Models.Npc;
    using Gamemode.Models.Spawn;
    using GTANetworkAPI;

    public class Vagos : Gang
    {
        public static readonly byte BlipColor = 46;
        public static readonly Spawn CarSpawn = new Spawn(new Vector3(330, -2042, 20.85), -42.1f);
        public static readonly Spawn CarSelection = new Spawn(new Vector3(319.44f, -2027.07f, 20.13f), -45.75f);
        public static readonly Spawn Spawn = new Spawn(new Vector3(336, -2054, 20.84), 3.16f);
        public static readonly Color Color = new Color(241, 204, 64);
        public static readonly int ColorClientSide = 126;

        public Vagos()
        {
            this.Name = "Vagos";
            this.GangColor = Color;
            this.PlayerSpawn = Spawn;
            this.CarMarker = new Marker(new Vector3(330, -2042, 20.8), this.GangColor, (MarkerType)36, "Car", new CarSelectionEvent(GangUtil.NpcIdVagos, CarSelection, ColorClientSide));
            this.ItemMarker = new Marker(new Vector3(327, -2049, 20.84), this.GangColor, (MarkerType)41, "Weapon", new ItemSelectionEvent(GangUtil.NpcIdVagos));
            this.Npc = new Npc(new Vector3(345.5, -2049, 21.6), 52.5f, "Старший", PedHash.Vagos01GFY, new Colshape.GangNpcEvent(NpcUtil.NpcNameVagos, GangUtil.NpcIdVagos));
            this.GangBlipColor = BlipColor;
        }
    }
}
