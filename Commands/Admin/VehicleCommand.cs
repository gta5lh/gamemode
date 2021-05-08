// <copyright file="Vehicle.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using System;
    using Gamemode.Commands.Admin;
    using Gamemode.Models.Admin;
    using Gamemode.Models.Player;
    using Gamemode.Models.Vehicle;
    using GTANetworkAPI;

    public class VehicleCommand : BaseCommandHandler
    {
        private const string CreateVehicleCommandUsage = "Использование: /vehicle {название}. Пример: /v buffalo";
        private const string FixVehicleCommandUsage = "Использование: /fixvehicle {vehicle_id}. Пример: /fv 10";
        private const string DeleteVehicleCommandUsage = "Использование: /deletevehicle {vehicle_id}. Пример: /dv 10";
        private readonly Random random = new Random();

        [Command("vehicle", CreateVehicleCommandUsage, Alias = "v", SensitiveInfo = true, Hide = true)]
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
            Vehicle vehicle = NAPI.Vehicle.CreateVehicle(vehicleHash, admin.Position, admin.Rotation.Z, 0, 0, "ADM");
            vehicle.CustomPrimaryColor = randomColor;
            vehicle.CustomSecondaryColor = randomColor;
            vehicle.Rotation = new Vector3(0, 0, admin.Heading);

            admin.SetIntoVehicle(vehicle, 0);

            string vehicleDisplayName = VehicleUtil.DisplayName(vehicle, vehicleName);
            AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} создал автомобиль {vehicleDisplayName.ToLower()} [{vehicle.Id}]");
            this.Logger.Warn($"Administrator {admin.Name} created vehicle {vehicleDisplayName}");
        }

        [Command("fixvehicle", FixVehicleCommandUsage, Alias = "fv", SensitiveInfo = true, Hide = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public void FixVehicle(CustomPlayer admin, string vehicleIdInput = null)
        {
            if (vehicleIdInput == null)
            {
                admin.SendChatMessage(FixVehicleCommandUsage);
                return;
            }

            ushort vehicleId;

            try
            {
                vehicleId = ushort.Parse(vehicleIdInput);
            }
            catch (Exception)
            {
                admin.SendChatMessage(DeleteVehicleCommandUsage);
                return;
            }

            Vehicle vehicle = VehicleUtil.GetById(vehicleId);
            if (vehicle == null)
            {
                admin.SendChatMessage($"Автомобиль с ID {vehicleId} не найден");
                return;
            }

            vehicle.Repair();

            string vehicleDisplayName = VehicleUtil.DisplayName(vehicle, string.Empty);
            AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} починил автомобиль {vehicleDisplayName.ToLower()} [{vehicle.Id}]");
            this.Logger.Warn($"Administrator {admin.Name} fixed vehicle {vehicleDisplayName}");
        }

        [Command("deletevehicle", DeleteVehicleCommandUsage, Alias = "dv", SensitiveInfo = true, Hide = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public void DeleteVehicle(CustomPlayer admin, string vehicleIdInput = null)
        {
            if (vehicleIdInput == null)
            {
                admin.SendChatMessage(DeleteVehicleCommandUsage);
                return;
            }

            ushort vehicleId;

            try
            {
                vehicleId = ushort.Parse(vehicleIdInput);
            }
            catch (Exception)
            {
                admin.SendChatMessage(DeleteVehicleCommandUsage);
                return;
            }

            CustomVehicle vehicle = (CustomVehicle)VehicleUtil.GetById(vehicleId);
            if (vehicle == null)
            {
                admin.SendChatMessage($"Автомобиль с ID {vehicleId} не найден");
                return;
            }

            CustomPlayer vehicleOwner = PlayerUtil.GetById(vehicle.OwnerPlayerId);
            if (vehicleOwner != null)
            {
                vehicleOwner.OneTimeVehicleId = null;
            }

            vehicle.Delete();

            string vehicleDisplayName = VehicleUtil.DisplayName(vehicle, string.Empty);
            AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} удалил автомобиль {vehicleDisplayName} [{vehicle.Id}]");
            this.Logger.Warn($"Administrator {admin.Name} deleted vehicle {vehicleDisplayName}");
        }
    }
}
