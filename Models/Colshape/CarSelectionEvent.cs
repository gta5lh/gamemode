// <copyright file="CarSelectionEvent.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Colshape
{
    using Gamemode.Models.Player;
    using Gamemode.Models.Spawn;
    using GTANetworkAPI;

    public class CarSelectionEvent : IColShapeEventHandler
    {
        private byte FractionId;
        private Spawn VehicleSpawnPosition;
        private int Color;

        public CarSelectionEvent(byte fractionId, Spawn vehicleSpawnPosition, int color)
        {
            this.FractionId = fractionId;
            this.VehicleSpawnPosition = vehicleSpawnPosition;
            this.Color = color;
        }

        public void OnEntityEnterColShape(ColShape shape, Player player)
        {
            CustomPlayer customPlayer = (CustomPlayer)player;
            if (customPlayer.Fraction != this.FractionId)
            {
                return;
            }

            if (customPlayer.IsInVehicle)
            {
                return;
            }

            NAPI.ClientEvent.TriggerClientEvent(player, "DisplayGangCarSelectionMenu", true, this.VehicleSpawnPosition, this.Color, this.FractionId, customPlayer.FractionRank);
        }

        public void OnEntityExitColShape(ColShape shape, Player player)
        {
            NAPI.ClientEvent.TriggerClientEvent(player, "DisplayGangCarSelectionMenu", false);
        }
    }
}
