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

            uint vehicleHash = NAPI.Util.GetHashKey(vehicleName);
            if (vehicleHash == 0)
            {
                admin.SendChatMessage($"Автомобиль с названием {vehicleName} отсутствует");
                return;
            }

            Color randomColor = new Color(this.random.Next(0, 255), this.random.Next(0, 255), this.random.Next(0, 255));
            Vehicle vehicle = NAPI.Vehicle.CreateVehicle(vehicleHash, admin.Position, admin.Rotation.Z, randomColor.ToInt32(), randomColor.ToInt32(), "ADM");
            admin.SetIntoVehicle(vehicle, 0);

            string vehicleDisplayName = vehicle.DisplayName != null ? vehicle.DisplayName : vehicleName;
            AdminsCache.SendMessageToAllAdmins($"{admin.Name} [{admin.StaticId}] создал автомобиль {vehicleDisplayName.ToLower()} [{vehicle.Id}]");
            this.Logger.Warn($"Administrator {admin.Name} created vehicle {vehicleDisplayName}");
        }

        private const string VehicleFixCommandUsage = "Использование: /vehiclefix {vehicle_id}. Пример: /vehiclefix 10";

        [Command("vehiclefix", VehicleFixCommandUsage, Alias = "vf", SensitiveInfo = true, GreedyArg = true, Hide = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public void VehicleFix(CustomPlayer admin, string vehicleIdInput = null)
        {
            if (vehicleIdInput == null)
            {
                admin.SendChatMessage(VehicleFixCommandUsage);
                return;
            }

            ushort vehicleId = 0;

            try
            {
                vehicleId = ushort.Parse(vehicleIdInput);
            }
            catch (Exception)
            {
                admin.SendChatMessage(VehicleFixCommandUsage);
                return;
            }

            Vehicle vehicle = VehicleUtil.GetById(vehicleId);
            if (vehicle == null)
            {
                admin.SendChatMessage($"Автомобиль с ID {vehicleId} не найден");
                return;
            }

            vehicle.Repair();

            string vehicleDisplayName = vehicle.DisplayName != null ? vehicle.DisplayName : string.Empty;
            AdminsCache.SendMessageToAllAdmins($"{admin.Name} [{admin.StaticId}] починил автомобиль {vehicleDisplayName.ToLower()} [{vehicle.Id}]");
            this.Logger.Warn($"Administrator {admin.Name} fixed vehicle {vehicleDisplayName}");
        }
    }
}
