// <copyright file="Vehicle.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
	using System;
	using Gamemode.Game.Admin;
	using Gamemode.Game.Admin.Commands;
	using Gamemode.Game.Admin.Models;
	using Gamemode.Game.Player.Models;
	using Gamemode.Game.Vehicle;
	using Gamemode.Game.Vehicle.Models;
	using GTANetworkAPI;

	public class Vehicle : BaseHandler
	{
		private const string CreateVehicleUsage = "Использование: /vehicle {название}. Пример: /v buffalo";

		private readonly Random random = new Random();

		[Command("vehicle", CreateVehicleUsage, Alias = "v", SensitiveInfo = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void OnVehicle(CPlayer admin, string? vehicleName = null, string? rInput = null, string? gInput = null, string? bInput = null)
		{
			if (vehicleName == null)
			{
				admin.SendChatMessage(CreateVehicleUsage);
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

			GTANetworkAPI.Vehicle vehicle = NAPI.Vehicle.CreateVehicle(vehicleHash, admin.Position, admin.Rotation.Z, 0, 0, "ADM");
			vehicle.CustomPrimaryColor = randomColorOne;
			vehicle.CustomSecondaryColor = randomColorTwo;
			vehicle.Rotation = new Vector3(0, 0, admin.Heading);
			admin.SetIntoVehicle(vehicle, 0);

			string vehicleDisplayName = VehicleUtil.DisplayName(vehicle, vehicleName);
			Cache.SendMessageToAllAdminsAction($"{admin.Name} создал автомобиль {vehicleDisplayName.ToLower()} [{vehicle.Id}]");
			this.Logger.Warn($"Administrator {admin.Name} created vehicle {vehicleDisplayName.ToLower()}");
		}

		private const string FixVehicleUsage = "Использование: /fixvehicle {vehicle_id}. Пример: /fv 10";

		[Command("fixvehicle", FixVehicleUsage, Alias = "fv", SensitiveInfo = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void OnFixVehicle(CPlayer admin, string? vehicleIdInput = null)
		{
			if (vehicleIdInput == null)
			{
				admin.SendChatMessage(FixVehicleUsage);
				return;
			}

			ushort vehicleId;

			try
			{
				vehicleId = ushort.Parse(vehicleIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(DeleteVehicleUsage);
				return;
			}

			GTANetworkAPI.Vehicle vehicle = VehicleUtil.GetById(vehicleId);
			if (vehicle == null)
			{
				admin.SendChatMessage($"Автомобиль с ID {vehicleId} не найден");
				return;
			}

			vehicle.Repair();

			string vehicleDisplayName = VehicleUtil.DisplayName(vehicle, string.Empty);
			Cache.SendMessageToAllAdminsAction($"{admin.Name} починил автомобиль {vehicleDisplayName.ToLower()} [{vehicle.Id}]");
			this.Logger.Warn($"Administrator {admin.Name} fixed vehicle {vehicleDisplayName.ToLower()}");
		}

		private const string DeleteVehicleUsage = "Использование: /deletevehicle {vehicle_id}. Пример: /dv 10";

		[Command("deletevehicle", DeleteVehicleUsage, Alias = "dv", SensitiveInfo = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void OnDeleteVehicle(CPlayer admin, string? vehicleIdInput = null)
		{
			if (vehicleIdInput == null)
			{
				admin.SendChatMessage(DeleteVehicleUsage);
				return;
			}

			ushort vehicleId;

			try
			{
				vehicleId = ushort.Parse(vehicleIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(DeleteVehicleUsage);
				return;
			}

			CVehicle vehicle = (CVehicle)VehicleUtil.GetById(vehicleId);
			if (vehicle == null)
			{
				admin.SendChatMessage($"Автомобиль с ID {vehicleId} не найден");
				return;
			}

			CPlayer vehicleOwner = PlayerUtil.GetById(vehicle.OwnerPlayerId);
			if (vehicleOwner != null)
			{
				vehicleOwner.OneTimeVehicleId = null;
			}

			string vehicleDisplayName = VehicleUtil.DisplayName(vehicle, string.Empty);
			vehicle.Delete();

			Cache.SendMessageToAllAdminsAction($"{admin.Name} удалил автомобиль {vehicleDisplayName.ToLower()} [{vehicle.Id}]");
			this.Logger.Warn($"Administrator {admin.Name} deleted vehicle {vehicleDisplayName.ToLower()}");
		}

		private const string FlipVehicleUsage = "Использование: /flipvehicle {vehicle_id}. Пример: /flv 10";

		[Command("flipvehicle", FlipVehicleUsage, Alias = "flv", SensitiveInfo = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void OnFlipVehicle(CPlayer admin, string? vehicleIdInput = null)
		{
			if (vehicleIdInput == null)
			{
				admin.SendChatMessage(FlipVehicleUsage);
				return;
			}

			ushort vehicleId;

			try
			{
				vehicleId = ushort.Parse(vehicleIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(FlipVehicleUsage);
				return;
			}

			CVehicle vehicle = (CVehicle)VehicleUtil.GetById(vehicleId);
			if (vehicle == null)
			{
				admin.SendChatMessage($"Автомобиль с ID {vehicleId} не найден");
				return;
			}

			vehicle.Rotation = new Vector3(0, 0, vehicle.Rotation.Z);

			string vehicleDisplayName = VehicleUtil.DisplayName(vehicle, string.Empty);
			Cache.SendMessageToAllAdminsAction($"{admin.Name} перевенул автомобиль {vehicleDisplayName.ToLower()} [{vehicle.Id}]");
			this.Logger.Warn($"Administrator {admin.Name} flipped vehicle {vehicleDisplayName.ToLower()}");
		}
	}
}
