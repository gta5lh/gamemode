// <copyright file="Ballas.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using GTANetworkAPI;

    public class Ballas : Gang
    {
        public Ballas()
        {
            this.Name = "Ballas";
            this.Color = new Color(125, 38, 205);
            this.CarMarker = new Marker(new Vector3(-419, 1130, 326), this.Color, (MarkerType)36, "Car", new CarSelectionEvent());
            this.ItemMarker = new Marker(new Vector3(-419, 1133, 326), this.Color, (MarkerType)41, "Weapon", new ItemSelectionEvent());
        }
    }
}
