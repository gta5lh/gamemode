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
            this.CarMarker = new Marker(new Vector3(84.7, -1972.8, 20.84), this.Color, (MarkerType)36, "Автомобиль", new CarSelectionEvent());
            this.ItemMarker = new Marker(new Vector3(102.9, -1959.3, 20.8), this.Color, (MarkerType)41, "Снаряжение", new ItemSelectionEvent());
        }
    }
}
