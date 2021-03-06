// <copyright file="Bloods.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using GTANetworkAPI;

    public class Bloods : Gang
    {
        public Bloods()
        {
            this.Name = "Bloods";
            this.Color = new Color(138, 3, 3);
            this.CarMarker = new Marker(new Vector3(-429, 1130, 326), this.Color, (MarkerType)36, "Car", new CarSelectionEvent());
            this.ItemMarker = new Marker(new Vector3(-429, 1133, 326), this.Color, (MarkerType)41, "Weapon", new ItemSelectionEvent());
        }
    }
}
