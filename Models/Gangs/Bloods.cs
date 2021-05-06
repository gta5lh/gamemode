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
        public static readonly Spawn Spawn = new Spawn(new Vector3(490, -1335, 29), -45f);
        public static readonly Color Color = new Color(138, 3, 3);

        public Bloods()
        {
            this.Name = "Bloods";
            this.GangColor = Color;
            this.PlayerSpawn = Spawn;
            this.CarMarker = new Marker(new Vector3(487, -1314, 29.2), this.GangColor, (MarkerType)36, "Car", new CarSelectionEvent());
            this.CarSpawn = new Spawn(new Vector3(489, -1313, 29.26), -74.2f);
            this.ItemMarker = new Marker(new Vector3(502, -1339, 29.26), this.GangColor, (MarkerType)41, "Weapon", new ItemSelectionEvent());
            this.Npc = new Npc(new Vector3(499, -1326.3, 29.33), 87.4f, "Старший", PedHash.Beach01AFM);
            this.BlipColor = 75;
        }
    }
}
