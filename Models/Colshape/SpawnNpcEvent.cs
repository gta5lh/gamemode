// <copyright file="CarSelectionEvent.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Colshape
{
    using Gamemode.Models.Player;
    using GTANetworkAPI;

    public class SpawnNpcEvent : IColShapeEventHandler
    {
        private string NpcName;

        public SpawnNpcEvent(string npcName)
        {
            this.NpcName = npcName;
        }

        public void OnEntityEnterColShape(ColShape shape, Player player)
        {
            CustomPlayer customPlayer = (CustomPlayer)player;
            if (customPlayer.IsInVehicle || customPlayer.Fraction != null)
            {
                return;
            }

            NAPI.ClientEvent.TriggerClientEvent(player, "DisplayPressE", true, this.NpcName);
        }

        public void OnEntityExitColShape(ColShape shape, Player player)
        {
            NAPI.ClientEvent.TriggerClientEvent(player, "DisplayPressE", false, this.NpcName);
        }
    }
}
