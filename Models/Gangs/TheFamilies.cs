// <copyright file="TheFamilies.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using GTANetworkAPI;

    public class TheFamilies : Gang
    {
        public TheFamilies()
        {
            this.Name = "The Families";
            this.Color = new Color(0, 255, 0);
            this.CarMarker = new Marker(new Vector3(-409, 1130, 326), this.Color, (MarkerType)36, "Car", new CarSelectionEvent());
            this.ItemMarker = new Marker(new Vector3(-409, 1133, 326), this.Color, (MarkerType)41, "Weapon", new ItemSelectionEvent());
        }
    }
}
