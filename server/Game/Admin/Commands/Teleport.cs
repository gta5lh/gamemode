﻿// <copyright file="Teleport.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode
{
	using System;
	using Gamemode.Game.Admin.Commands;
	using Gamemode.Game.Admin.Models;
	using Gamemode.Game.Player.Models;
	using Gamemode.Game.Vehicle;
	using GTANetworkAPI;

	public class Teleport : Script
	{
		private const string TeleportUsage = "Использование: /tp {player_id} | /tp {player_id} {player_id}. Пример: [/tp 2], [/tp 2 10]";

		[Command("teleport", TeleportUsage, Alias = "tp", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public static void OnTeleport(CPlayer admin, string? firstPlayerIdInput = null, string? secondPlayerIdInput = null)
		{
			if (firstPlayerIdInput == null)
			{
				admin.SendChatMessage(TeleportUsage);
				return;
			}

			ushort firstPlayerId;
			ushort secondPlayerId;

			try
			{
				firstPlayerId = ushort.Parse(firstPlayerIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(TeleportUsage);
				return;
			}

			if (secondPlayerIdInput != null)
			{
				try
				{
					secondPlayerId = ushort.Parse(secondPlayerIdInput);
				}
				catch (Exception)
				{
					admin.SendChatMessage(TeleportUsage);
					return;
				}

				CPlayer firstPlayer = PlayerUtil.GetById(firstPlayerId);
				if (firstPlayer == null)
				{
					admin.SendChatMessage($"Пользователь с DID {firstPlayerId} не найден");
					return;
				}

				CPlayer secondPlayer = PlayerUtil.GetById(secondPlayerId);
				if (secondPlayer == null)
				{
					admin.SendChatMessage($"Пользователь с DID {secondPlayerId} не найден");
					return;
				}

				firstPlayer.Position = secondPlayer.Position;
				admin.SendChatMessage($"Вы телепортировали {firstPlayer.Name} [{firstPlayer.Id}] к {secondPlayer.Name} [{secondPlayer.Id}]");
				return;
			}

			CPlayer targetPlayer = PlayerUtil.GetById(firstPlayerId);
			if (targetPlayer == null)
			{
				admin.SendChatMessage($"Пользователь с DID {firstPlayerId} не найден");
				return;
			}

			admin.Position = targetPlayer.Position;
			admin.SendChatMessage($"Вы телепортировались к {targetPlayer.Name} [{targetPlayer.Id}]");
		}

		private const string GetCarUsage = "Использование: /getcar {id}. Пример: /getcar 0";

		[Command("getcar", GetCarUsage, Alias = "gc", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public static void OnGetCar(CPlayer admin, string? vehicleIdInput = null)
		{
			if (vehicleIdInput == null)
			{
				admin.SendChatMessage(GetCarUsage);
				return;
			}

			ushort vehicleId;

			try
			{
				vehicleId = ushort.Parse(vehicleIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(GetCarUsage);
				return;
			}

			GTANetworkAPI.Vehicle vehicle = VehicleUtil.GetById(vehicleId);
			if (vehicle == null)
			{
				admin.SendChatMessage($"Автомобиль с ID {vehicleIdInput} отсутствует");
				return;
			}

			vehicle.Position = admin.Position;
			vehicle.Rotation = admin.Rotation;
			admin.SendChatMessage($"Вы телепортировали автомобиль {vehicleIdInput} к себе");
		}

		// TODO
		// private Spawn[] spawns = new Spawn[] {
		//  Bloods.Spawn,
		//  Ballas.Spawn,
		//  TheFamilies.Spawn,
		//  Vagos.Spawn,
		//  Marabunta.Spawn,
		//  PlayerSpawns.SpawnPositions[0],
		//  PlayerSpawns.SpawnPositions[1],
		//  PlayerSpawns.SpawnPositions[2],
		// };
		private const string TeleportLocationUsage = "Использование: /tpl {location_id}. Пример: [/tpl 0]";

		[Command("teleportlocation", TeleportUsage, Alias = "tpl", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public static void OnTeleport(CPlayer admin, string? locationIdInput = null)
		{
			if (locationIdInput == null)
			{
				admin.SendChatMessage(TeleportLocationUsage);
				return;
			}

			ushort locationId;

			try
			{
				locationId = ushort.Parse(locationIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(TeleportLocationUsage);
				return;
			}

			// TODO
			// if (locationId < 0 || locationId >= spawns.Length)
			// {
			//  admin.SendChatMessage($"Максимальный ID локации = {spawns.Length - 1}");
			//  return;
			// }

			// Spawn spawn = spawns[locationId];
			// admin.Position = spawn.Position;
			// admin.Heading = spawn.Heading;
			// admin.SendChatMessage($"Вы телепортировались в локацию {locationId}");
		}

		[Command("teleportwaypoint", TeleportUsage, Alias = "tpw", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public static void OnTeleportWaypoint(CPlayer admin)
		{
			NAPI.ClientEvent.TriggerClientEvent(admin, "TeleportToWaypoint");
		}

		private const string TeleportInteriorUsage = "Использование: /tpi {name} {x} {y} {z}. Пример: [/tpi 0apa_v_mp_h_01_a -786.8663, 315.7642, 217.6385]";

		[Command("teleportinterior", TeleportUsage, Alias = "tpi", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public static void OnTeleportInterior(CPlayer admin, string? interiorNameInput = null, string xInput = "0", string yInput = "0", string zInput = "0")
		{
			if (interiorNameInput == null)
			{
				admin.SendChatMessage(TeleportInteriorUsage);
				return;
			}

			float x, y, z = 0;

			try
			{
				x = float.Parse(xInput);
				y = float.Parse(yInput);
				z = float.Parse(zInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(TeleportInteriorUsage);
				return;
			}

			NAPI.ClientEvent.TriggerClientEvent(admin, "TeleportToInterior", interiorNameInput, x, y, z);
			admin.SendChatMessage($"Вы телепортировались в интерьер {interiorNameInput}");
		}
	}
}
