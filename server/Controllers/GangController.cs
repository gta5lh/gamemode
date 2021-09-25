// <copyright file="GangController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
	using System.Collections.Generic;
	using System.Threading.Tasks;
	using Gamemode.ApiClient.Models;
	using Gamemode.Models.Player;
	using Gamemode.Models.Spawn;
	using Gamemode.Models.Vehicle;
	using GamemodeCommon.Models.Data;
	using GamemodeCommon.Models;
	using GTANetworkAPI;
	using Newtonsoft.Json;
	using Rpc.User;
	using System;
	using Rollbar;
	using Gamemode.Services;

	public class GangController : Script
	{
		private readonly List<GangVehicle> GangVehicles;

		private readonly Ballas ballas;
		private readonly Bloods bloods;
		private readonly Marabunta marabunta;
		private readonly TheFamilies theFamilies;
		private readonly Vagos vagos;

		public GangController()
		{
			this.ballas = new Ballas();
			this.bloods = new Bloods();
			this.marabunta = new Marabunta();
			this.theFamilies = new TheFamilies();
			this.vagos = new Vagos();

			this.GangVehicles = new List<GangVehicle>
			{
				new GangVehicle(0x43779C54, 1),
				new GangVehicle(0x6D19CCBC, 1),
				new GangVehicle(0x1BB290BC, 1),
				new GangVehicle(0x81634188, 2),
				new GangVehicle(0xD7278283, 2),
				new GangVehicle(0x779B4F2D, 3),
				new GangVehicle(0x59E0FBF3, 4),
				new GangVehicle(0x1F52A43F, 4),
				new GangVehicle(0x11F76C14, 5),
				new GangVehicle(0x2C509634, 5),
				new GangVehicle(0x6F946279, 6),
				new GangVehicle(0x81A9CDDF, 6),
				new GangVehicle(0x51D83328, 6),
				new GangVehicle(0xD756460C, 7),
				new GangVehicle(0xF26CEFF9, 7),
				new GangVehicle(0xA4A4E453, 8),
				new GangVehicle(0x86FE0B60, 8),
				new GangVehicle(0x94B395C5, 9),
				new GangVehicle(0x42F2ED16, 10),
				new GangVehicle(0x462FE277, 10),
				new GangVehicle(0x4CE68AC, 10)
			};
		}

		[ServerEvent(Event.ResourceStartEx)]
		private void ResourceStartEx(string resourceName)
		{
			this.ballas.Create();
			this.bloods.Create();
			this.marabunta.Create();
			this.theFamilies.Create();
			this.vagos.Create();
		}

		[RemoteProc("PlayerSelectedGangCar")]
		private void OnPlayerSelectedGangCar(CustomPlayer player, uint vehicleModel)
		{
			string gangName = GangUtil.GangNameById[player.Fraction.Value];
			Spawn carSpawn = GangUtil.CarSpawnByGangId[player.Fraction.Value];
			Color gangColor = GangUtil.GangColorByName[gangName];

			if (player.OneTimeVehicleId != null)
			{
				VehicleUtil.GetById(player.OneTimeVehicleId.Value).Delete();
			}

			CustomVehicle vehicle = (CustomVehicle)NAPI.Vehicle.CreateVehicle(vehicleModel, carSpawn.Position, carSpawn.Heading, 0, 0, gangName);
			vehicle.OwnerPlayerId = player.Id;
			vehicle.CustomPrimaryColor = gangColor;
			vehicle.CustomSecondaryColor = gangColor;
			vehicle.Rotation = new Vector3(0, 0, carSpawn.Heading);

			player.Dimension = 0;
			player.SetIntoVehicle(vehicle, 0);
			player.OneTimeVehicleId = vehicle.Id;
			vehicle.SetSharedData(DataKey.VehicleCollisionDisabled, true);
		}

		[RemoteProc("PlayerSelectedGangItem", true)]
		private async Task<System.Object> PlayerSelectedGangItem(CustomPlayer player, string itemName, bool isAmmo)
		{
			int price = 0;
			if (!WeaponService.PriceByItemName.TryGetValue(itemName, out price))
			{
				return JsonConvert.SerializeObject(new GangItemResponse(NotificationType.Error, "Предмет недоступен в магазине :("));
			}

			if ((player.Money - price) < 0)
			{
				return JsonConvert.SerializeObject(new GangItemResponse(NotificationType.Error, "Недостаточно денег!"));
			}

			WeaponHash weaponHash = (WeaponHash)NAPI.Util.GetHashKey(itemName);
			int amount = 0;

			if (isAmmo)
			{
				if (!WeaponService.AmountByAmmoType.TryGetValue(itemName, out amount))
				{
					return JsonConvert.SerializeObject(new GangItemResponse(NotificationType.Error, "Выбранный тип патрон недоступен в магазине :("));
				}

				List<WeaponHash>? weaponHashesByAmmoType = WeaponService.WeaponHashesByAmmoType(itemName);
				if (weaponHashesByAmmoType == null)
				{
					RollbarLocator.RollbarInstance.Error(new ArgumentOutOfRangeException(itemName));
					return JsonConvert.SerializeObject(new GangItemResponse(NotificationType.Error, "Патроны недоступны в магазине :("));
				}

				bool found = false;

				foreach (WeaponHash wh in weaponHashesByAmmoType)
				{
					if (player.HasWeapon(wh))
					{
						weaponHash = wh;
						found = true;
						break;
					}
				}

				if (!found)
				{
					return JsonConvert.SerializeObject(new GangItemResponse(NotificationType.Error, "У тебя отсутвует оружие для выбранного типа патрон!"));
				}
			}
			else
			{
				if (player.HasWeapon(weaponHash))
				{
					return JsonConvert.SerializeObject(new GangItemResponse(NotificationType.Error, "У тебя уже имеется выбранное оружие!"));
				}
			}

			int maxAmount = 9999;
			if (WeaponHash.Flaregun == weaponHash)
			{
				maxAmount = 20;
			}

			if ((player.GetWeaponAmmo(weaponHash) + amount) > maxAmount)
			{
				return JsonConvert.SerializeObject(new GangItemResponse(NotificationType.Error, "Патрон слишком много!"));
			}

			player.Money -= price;
			player.CustomGiveWeapon(weaponHash, amount);
			return JsonConvert.SerializeObject(new GangItemResponse(NotificationType.Success, "Успешная покупка!"));
		}

		[ServerEvent(Event.PlayerDeath)]
		private void OnPlayerDeath(CustomPlayer target, CustomPlayer killer, uint reason)
		{
			if (target.Fraction == null)
			{
				return;
			}

			target.CustomRemoveAllWeapons();

			foreach (Weapon weapon in GangUtil.WeaponsByGangId[target.Fraction.Value])
			{
				target.CustomGiveWeapon((WeaponHash)weapon.Hash, weapon.Amount);
			}
		}

		[ServerEvent(Event.PlayerConnected)]
		private void OnPlayerConnected(Player player)
		{
			NAPI.ClientEvent.TriggerClientEvent(player, "SetGangVehicles", this.GangVehicles);
		}
	}

	public class PlayerSelectedGangCarRequest
	{
		public string CarName { get; set; }
	}
}
