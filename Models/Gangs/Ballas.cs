// <copyright file="Ballas.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using Gamemode.Models.Npc;
    using Gamemode.Models.Spawn;
    using GTANetworkAPI;

    public class Ballas : Gang
    {
        public static readonly Vector3 SpawnLocation = new Vector3(89.6, -1952.5, 20.75);

        public Ballas()
        {
            this.Name = "Ballas";
            this.Color = new Color(125, 38, 205);
            this.Spawn = new Spawn(SpawnLocation, -37.86f);
            this.CarMarker = new Marker(new Vector3(84.7, -1972.8, 20.84), this.Color, (MarkerType)36, "Автомобиль", new CarSelectionEvent());
            this.CarSpawn = new Spawn(new Vector3(88.7, -1967.8, 20.75), -39.1f);
            this.ItemMarker = new Marker(new Vector3(102.9, -1959.3, 20.8), this.Color, (MarkerType)41, "Снаряжение", new ItemSelectionEvent());
            this.Npc = new Npc(new Vector3(86.7, -1950, 20.8), -54, "Старший", PedHash.Beach01AFM);
        }
    }
}
