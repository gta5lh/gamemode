// <copyright file="TheFamilies.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using Gamemode.Models.Npc;
    using Gamemode.Models.Spawn;
    using GTANetworkAPI;

    public class TheFamilies : Gang
    {
        public static readonly Vector3 SpawnLocation = new Vector3(-14, -1446, 30.65);

        public TheFamilies()
        {
            this.Name = "The Families";
            this.Color = new Color(0, 255, 0);
            this.Spawn = new Spawn(SpawnLocation, 179);
            this.CarMarker = new Marker(new Vector3(-25, -1433, 30.65), this.Color, (MarkerType)36, "Car", new CarSelectionEvent());
            this.CarSpawn = new Spawn(new Vector3(-24, -1436, 30.65), -179f);
            this.ItemMarker = new Marker(new Vector3(-10, -1445, 30.75), this.Color, (MarkerType)41, "Weapon", new ItemSelectionEvent());
            this.Npc = new Npc(new Vector3(-18, -1448, 30.65), -48, "Старший", PedHash.Beach01AFM);
        }
    }
}
