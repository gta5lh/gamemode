// <copyright file="Marabunta.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using GTANetworkAPI;

    public class Marabunta : Gang
    {
        public Marabunta()
        {
            this.Name = "Marabunta";
            this.Color = new Color(0, 118, 215);
            this.CarMarker = new Marker(new Vector3(-424, 1130, 326), this.Color, (MarkerType)36, "Car", new CarSelectionEvent());
            this.ItemMarker = new Marker(new Vector3(-424, 1133, 326), this.Color, (MarkerType)41, "Weapon", new ItemSelectionEvent());
        }
    }
}
