// <copyright file="Vehicle.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
	using System;
	using Gamemode.Commands.Admin;
	using Gamemode.Controllers;
	using Gamemode.Models.Admin;
	using Gamemode.Models.Player;
	using Gamemode.Models.Vehicle;
	using GTANetworkAPI;
	using Gamemode.Cache.Player;

	public class VehicleCommand : BaseCommandHandler
	{
		private const string CreateVehicleCommandUsage = "Использование: /vehicle {название}. Пример: /v buffalo";
		private const string FixVehicleCommandUsage = "Использование: /fixvehicle {vehicle_id}. Пример: /fv 10";
		private const string DeleteVehicleCommandUsage = "Использование: /deletevehicle {vehicle_id}. Пример: /dv 10";
		private readonly Random random = new Random();

		[Command("vehicle", CreateVehicleCommandUsage, Alias = "v", SensitiveInfo = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void Vehicle(CustomPlayer admin, string vehicleName = null, string rInput = null, string gInput = null, string bInput = null)
		{
			if (vehicleName == null)
			{
				admin.SendChatMessage(CreateVehicleCommandUsage);
				return;
			}

			Color randomColorOne = new Color(this.random.Next(0, 255), this.random.Next(0, 255), this.random.Next(0, 255));
			Color randomColorTwo = new Color(this.random.Next(0, 255), this.random.Next(0, 255), this.random.Next(0, 255));

			if (rInput != null && gInput != null && bInput != null)
			{
				int r;
				int g;
				int b;

				try
				{
					r = int.Parse(rInput);
					g = int.Parse(gInput);
					b = int.Parse(bInput);
				}
				catch
				{
					admin.SendChatMessage("Введите цвета rgb через пробел. От 0 до 255. Пример: /veh infernus 255 255 255");
					return;
				}

				randomColorOne = new Color(r, g, b);
				randomColorTwo = randomColorOne;
			}

			uint vehicleHash = NAPI.Util.GetHashKey(vehicleName);
			if (vehicleHash == 0)
			{
				admin.SendChatMessage($"Автомобиль с названием {vehicleName} отсутствует");
				return;
			}

			Vehicle vehicle = NAPI.Vehicle.CreateVehicle(vehicleHash, admin.Position, admin.Rotation.Z, 0, 0, "ADM");
			vehicle.CustomPrimaryColor = randomColorOne;
			vehicle.CustomSecondaryColor = randomColorTwo;
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
