// <copyright file="Vehicle.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using GTANetworkAPI;

    public class Vehicle : Script
    {
        [Command("vehicle", "Usage: /vehicle {name}", Alias = "v", SensitiveInfo = true, GreedyArg = true)]
        public void CMDVehicle(Player player, string vehicleName)
        {
            GTANetworkAPI.Vehicle vehicle = NAPI.Vehicle.CreateVehicle(NAPI.Util.VehicleNameToModel(vehicleName), player.Position, 0, new Color(0, 255, 0), new Color(0, 255, 0), "LOH");
            player.SetIntoVehicle(vehicle, 0);
        }
    }
}
