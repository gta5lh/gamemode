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
        public static readonly Spawn Spawn = new Spawn(new Vector3(336, -2054, 20.84), 3.16f);
        public static readonly Color Color = new Color(255, 243, 63);

        public Vagos()
        {
            this.Name = "Vagos";
            this.GangColor = Color;
            this.PlayerSpawn = Spawn;
            this.CarMarker = new Marker(new Vector3(330, -2042, 20.8), this.GangColor, (MarkerType)36, "Car", new CarSelectionEvent());
            this.CarSpawn = new Spawn(new Vector3(330, -2042, 20.85), -42.1f);
            this.ItemMarker = new Marker(new Vector3(327, -2049, 20.84), this.GangColor, (MarkerType)41, "Weapon", new ItemSelectionEvent());
            this.Npc = new Npc(new Vector3(345.5, -2049, 21.6), 52.5f, "Старший", PedHash.Vagos01GFY);
            this.BlipColor = 46;
        }
    }
}
