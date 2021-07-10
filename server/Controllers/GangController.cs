// <copyright file="GangController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
	using System.Collections.Generic;
	using Gamemode.ApiClient.Models;
	using Gamemode.Models.Player;
	using Gamemode.Models.Spawn;
	using Gamemode.Models.Vehicle;
	using GTANetworkAPI;
	using Newtonsoft.Json;
	using Rpc.User;

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

		[RemoteEvent("PlayerSelectedGangCar")]
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
		}

		[RemoteEvent("PlayerSelectedGangItem")]
		private void PlayerSelectedGangItem(CustomPlayer player, string request)
		{
			PlayerSelectedGangItemRequest playerSelectedGangItemRequest = JsonConvert.DeserializeObject<PlayerSelectedGangItemRequest>(request);

			uint itemHash = NAPI.Util.GetHashKey(playerSelectedGangItemRequest.ItemName);
			player.CustomGiveWeapon((WeaponHash)itemHash, 100);
		}

		[ServerEvent(Event.PlayerDeath)]
		private async void OnPlayerDeath(CustomPlayer target, CustomPlayer killer, uint reason)
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

	public class GangVehicle
	{
		public GangVehicle(uint model, byte rank)
		{
			Model = model;
			Rank = rank;
		}

		public uint Model { get; set; }

		public byte Rank { get; set; }
	}

	public class PlayerSelectedGangCarRequest
	{
		public string CarName { get; set; }
	}

	public class PlayerSelectedGangItemRequest
	{
		public string ItemName { get; set; }
	}
}
