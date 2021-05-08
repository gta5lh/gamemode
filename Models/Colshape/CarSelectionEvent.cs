// <copyright file="CarSelectionEvent.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Colshape
{
    using Gamemode.Models.Player;
    using GTANetworkAPI;

    public class CarSelectionEvent : IColShapeEventHandler
    {
        private byte FractionId;

        public CarSelectionEvent(byte fractionId)
        {
            this.FractionId = fractionId;
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

            NAPI.ClientEvent.TriggerClientEvent(player, "DisplayGangCarSelectionMenu", true);
        }

        public void OnEntityExitColShape(ColShape shape, Player player)
        {
            NAPI.ClientEvent.TriggerClientEvent(player, "DisplayGangCarSelectionMenu", false);
        }
    }
}
