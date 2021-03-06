// <copyright file="Vagos.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using GTANetworkAPI;

    public class Vagos : Gang
    {
        public Vagos()
        {
            this.Name = "Vagos";
            this.Color = new Color(255, 243, 63);
            this.CarMarker = new Marker(new Vector3(-414, 1130, 326), this.Color, (MarkerType)36, "Car", new CarSelectionEvent());
            this.ItemMarker = new Marker(new Vector3(-414, 1133, 326), this.Color, (MarkerType)41, "Weapon", new ItemSelectionEvent());
        }
    }
}
