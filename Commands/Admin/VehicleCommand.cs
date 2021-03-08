// <copyright file="Vehicle.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using System;
    using Gamemode.Commands.Admin;
    using Gamemode.Models.Admin;
    using Gamemode.Models.Player;
    using GTANetworkAPI;

    public class VehicleCommand : BaseCommandHandler
    {
        private const string CreateVehicleCommandUsage = "Использование: /vehicle {название}. Пример: /vehicle buffalo";
        private readonly Random random = new Random();

        [Command("vehicle", CreateVehicleCommandUsage, Alias = "v", SensitiveInfo = true, GreedyArg = true, Hide = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public void Vehicle(CustomPlayer admin, string vehicleName = null)
        {
            if (vehicleName == null)
            {
                admin.SendChatMessage(CreateVehicleCommandUsage);
                return;
            }

            VehicleHash vehicleHash = NAPI.Util.VehicleNameToModel(vehicleName);
            if (vehicleHash == 0)
            {
                admin.SendChatMessage($"Автомобиль с названием {vehicleName} отсутствует");
                return;
            }

            Color randomColor = new Color(this.random.Next(0, 255), this.random.Next(0, 255), this.random.Next(0, 255));
            Vehicle vehicle = NAPI.Vehicle.CreateVehicle(vehicleHash, admin.Position, 0, randomColor, randomColor, "ADM");
            admin.SetIntoVehicle(vehicle, 0);

            string vehicleDisplayName = vehicle.DisplayName != null ? vehicle.DisplayName : vehicleName;
            AdminsCache.SendMessageToAllAdmins($"{admin.Name} создал автомобиль {vehicleDisplayName.ToLower()} [{vehicle.Id}]");
            this.Logger.Warn($"Administrator {admin.Name} created vehicle {vehicle.DisplayName}");
        }
    }
}
