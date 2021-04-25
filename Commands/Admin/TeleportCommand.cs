// <copyright file="Teleport.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using System;
    using Gamemode.Commands.Admin;
    using Gamemode.Models.Admin;
    using Gamemode.Models.Player;
    using Gamemode.Models.Spawn;
    using GTANetworkAPI;

    public class TeleportCommand : Script
    {
        private const string TeleportCommandUsage = "Использование: /tp {player_id} | /tp {player_id} {player_id}. Пример: [/tp 2], [/tp 2 10]";

        [Command("teleport", TeleportCommandUsage, Alias = "tp", SensitiveInfo = true, GreedyArg = true, Hide = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public void Teleport(CustomPlayer admin, string firstPlayerIdInput = null, string secondPlayerIdInput = null)
        {
            if (firstPlayerIdInput == null)
            {
                admin.SendChatMessage(TeleportCommandUsage);
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
                admin.SendChatMessage(TeleportCommandUsage);
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
                    admin.SendChatMessage(TeleportCommandUsage);
                    return;
                }

                CustomPlayer firstPlayer = PlayerUtil.GetById(firstPlayerId);
                if (firstPlayer == null)
                {
                    admin.SendChatMessage($"Пользователь с DID {firstPlayerId} не найден");
                    return;
                }

                CustomPlayer secondPlayer = PlayerUtil.GetById(secondPlayerId);
                if (secondPlayer == null)
                {
                    admin.SendChatMessage($"Пользователь с DID {secondPlayerId} не найден");
                    return;
                }

                firstPlayer.Position = secondPlayer.Position;
                admin.SendChatMessage($"Вы телепортировали {firstPlayer.Name} [{firstPlayer.Id}] к {secondPlayer.Name} [{secondPlayer.Id}]");
                return;
            }

            CustomPlayer targetPlayer = PlayerUtil.GetById(firstPlayerId);
            if (targetPlayer == null)
            {
                admin.SendChatMessage($"Пользователь с DID {firstPlayerId} не найден");
                return;
            }

            admin.Position = targetPlayer.Position;
            admin.SendChatMessage($"Вы телепортировались к {targetPlayer.Name} [{targetPlayer.Id}]");
        }

        private const string GetCarCommandUsage = "Использование: /getcar {id}. Пример: /getcar 0";

        [Command("getcar", GetCarCommandUsage, Alias = "gc", SensitiveInfo = true, GreedyArg = true, Hide = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public void GetCar(CustomPlayer admin, string vehicleIdInput = null)
        {
            if (vehicleIdInput == null)
            {
                admin.SendChatMessage(GetCarCommandUsage);
                return;
            }

            ushort vehicleId;

            try
            {
                vehicleId = ushort.Parse(vehicleIdInput);
            }
            catch (Exception)
            {
                admin.SendChatMessage(GetCarCommandUsage);
                return;
            }

            Vehicle vehicle = VehicleUtil.GetById(vehicleId);
            if (vehicle == null)
            {
                admin.SendChatMessage($"Автомобиль с ID {vehicleIdInput} отсутствует");
                return;
            }

            vehicle.Position = admin.Position;
            vehicle.Rotation = admin.Rotation;
            admin.SendChatMessage($"Вы телепортировали автомобиль {vehicleIdInput} к себе");
        }

        private Spawn[] spawns = new Spawn[] {
            Bloods.Spawn,
            Ballas.Spawn,
            TheFamilies.Spawn,
            Vagos.Spawn,
            Marabunta.Spawn,
        };

        private const string TeleportLocationCommandUsage = "Использование: /tpl {location_id}. Пример: [/tpl 0]";

        [Command("teleportlocation", TeleportCommandUsage, Alias = "tpl", SensitiveInfo = true, GreedyArg = true, Hide = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public void Teleport(CustomPlayer admin, string locationIdInput = null)
        {
            if (locationIdInput == null)
            {
                admin.SendChatMessage(TeleportLocationCommandUsage);
                return;
            }

            ushort locationId;

            try
            {
                locationId = ushort.Parse(locationIdInput);
            }
            catch (Exception)
            {
                admin.SendChatMessage(TeleportLocationCommandUsage);
                return;
            }

            if (locationId < 0 || locationId >= spawns.Length)
            {
                admin.SendChatMessage($"Максимальный ID локации = {spawns.Length-1}");
                return;
            }

            Spawn spawn = spawns[locationId];
            admin.Position = spawn.Position;
            admin.Heading = spawn.Heading;
            admin.SendChatMessage($"Вы телепортировались в локацию {locationId}");
        }
    }
}
