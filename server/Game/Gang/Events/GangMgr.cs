﻿// <copyright file="GangMgr.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Gang.Events
{
	using System;
	using System.Collections.Generic;
	using Gamemode.Game.Gang;
	using Gamemode.Game.Player.Models;
	using Gamemode.Game.Spawn;
	using Gamemode.Game.Vehicle.Models;
	using GamemodeCommon.Models;
	using GamemodeCommon.Models.Data;
	using GTANetworkAPI;
	using Newtonsoft.Json;
	using Rollbar;

	public class GangMgr : Script
	{
		private readonly List<GangVehicle> gangVehicles;

		private readonly Ballas ballas;
		private readonly Bloods bloods;
		private readonly Marabunta marabunta;
		private readonly TheFamilies theFamilies;
		private readonly Vagos vagos;

		public GangMgr()
		{
			this.ballas = new Ballas();
			this.bloods = new Bloods();
			this.marabunta = new Marabunta();
			this.theFamilies = new TheFamilies();
			this.vagos = new Vagos();

			this.gangVehicles = new List<GangVehicle>
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
				new GangVehicle(0x4CE68AC, 10),
			};
		}

		[ServerEvent(Event.PlayerDeath)]
		private static void OnPlayerDeath(CPlayer target, CPlayer killer, uint reason)
		{
			if (target.Fraction == null)
			{
				return;
			}

			target.CustomRemoveAllWeapons();

			foreach (Rpc.Player.Weapon weapon in Util.WeaponsByGangId[target.Fraction.Value])
			{
				target.CustomGiveWeapon((WeaponHash)weapon.Hash, weapon.Amount);
			}
		}

		[RemoteProc("PlayerSelectedGangCar")]
		private static void OnPlayerSelectedGangCar(CPlayer player, uint vehicleModel)
		{
			string gangName = Util.GangNameById[player.Fraction!.Value];
			Spawn carSpawn = Util.CarSpawnByGangId[player.Fraction.Value];
			Color gangColor = Util.GangColorByName[gangName];

			if (player.OneTimeVehicleId != null)
			{
				Game.Vehicle.Util.GetById(player.OneTimeVehicleId.Value).Delete();
			}

			CVehicle vehicle = (CVehicle)NAPI.Vehicle.CreateVehicle(vehicleModel, carSpawn.Position, carSpawn.Heading, 0, 0, gangName);
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
		private static object PlayerSelectedGangItem(CPlayer player, string itemName, bool isAmmo)
		{
			if (!Weapon.Weapon.PriceByItemName.TryGetValue(itemName, out int price))
			{
				return JsonConvert.SerializeObject(new GangItemResponse(NotificationType.Error, "Предмет недоступен в магазине :("));
			}

			if ((player.Money - price) < 0)
			{
				return JsonConvert.SerializeObject(new GangItemResponse(NotificationType.Error, "Недостаточно денег!"));
			}

			if (itemName == "armor")
			{
				if (player.Armor >= 100)
				{
					return JsonConvert.SerializeObject(new GangItemResponse(NotificationType.Error, "Бронежилет уже имеется!"));
				}

				player.Armor = 100;
				player.Money -= price;
				return JsonConvert.SerializeObject(new GangItemResponse(NotificationType.Success, "Успешная покупка!"));
			}

			if (itemName == "health")
			{
				if (player.Health >= 100)
				{
					return JsonConvert.SerializeObject(new GangItemResponse(NotificationType.Error, "Здоровье уже восполнено!"));
				}

				player.Health = 100;
				player.Money -= price;
				return JsonConvert.SerializeObject(new GangItemResponse(NotificationType.Success, "Успешная покупка!"));
			}

			WeaponHash weaponHash = (WeaponHash)NAPI.Util.GetHashKey(itemName);
			int amount = 0;
			List<WeaponHash> ammoForWeapons = new();

			if (isAmmo)
			{
				if (!Weapon.Weapon.AmountByAmmoType.TryGetValue(itemName, out amount))
				{
					return JsonConvert.SerializeObject(new GangItemResponse(NotificationType.Error, "Выбранный тип патрон недоступен в магазине :("));
				}

				List<WeaponHash>? weaponHashesByAmmoType = Weapon.Weapon.WeaponHashesByAmmoType(itemName);
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
						found = true;
						ammoForWeapons.Add(wh);
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
			if (weaponHash == WeaponHash.Flaregun)
			{
				maxAmount = 20;
			}

			if ((player.GetWeaponAmmo(weaponHash) + amount) > maxAmount)
			{
				return JsonConvert.SerializeObject(new GangItemResponse(NotificationType.Error, "Патрон слишком много!"));
			}

			player.Money -= price;

			if (isAmmo)
			{
				int lowestAmount = player.GetWeaponAmmo(ammoForWeapons[0]);
				WeaponHash weaponToGiveAmount = ammoForWeapons[0];
				foreach (WeaponHash wh in ammoForWeapons)
				{
					int a = player.GetWeaponAmmo(wh);
					if (a < lowestAmount)
					{
						lowestAmount = a;
						weaponToGiveAmount = wh;
					}
				}

				player.CustomGiveWeapon(weaponToGiveAmount, amount);
				amount = player.GetWeaponAmmo(weaponToGiveAmount);

				foreach (WeaponHash wh in ammoForWeapons)
				{
					player.SetWeaponAmmo(wh, amount);
				}
			}
			else
			{
				string ammoType = Weapon.Weapon.AmmoTypeByWeapon[weaponHash];
				List<WeaponHash> weapons = Weapon.Weapon.WeaponsByAmmoType[ammoType];

				WeaponHash? existingWeaponHash = null;
				int lowestAmount = int.MaxValue;

				foreach (WeaponHash wh in weapons)
				{
					if (player.HasWeapon(wh))
					{
						int a = player.GetWeaponAmmo(wh);
						if (a < lowestAmount)
						{
							lowestAmount = a;
							existingWeaponHash = wh;
						}
					}
				}

				amount = 0;
				if (existingWeaponHash != null)
				{
					amount = player.GetWeaponAmmo(existingWeaponHash.Value);
				}

				player.CustomGiveWeapon(weaponHash, 0);
				player.SetWeaponAmmo(weaponHash, amount);
			}

			return JsonConvert.SerializeObject(new GangItemResponse(NotificationType.Success, "Успешная покупка!"));
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

		[ServerEvent(Event.PlayerConnected)]
		private void OnPlayerConnected(GTANetworkAPI.Player player)
		{
			NAPI.ClientEvent.TriggerClientEvent(player, "SetGangVehicles", this.gangVehicles);
		}
	}
}
