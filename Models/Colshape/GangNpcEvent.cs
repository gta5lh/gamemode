// <copyright file="CarSelectionEvent.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

using GTANetworkAPI;

namespace Gamemode.Colshape
{
    public class GangNpcEvent : IColShapeEventHandler
    {
        private string NpcName;

        public GangNpcEvent(string npcName)
        {
            this.NpcName = npcName;
        }

        public void OnEntityEnterColShape(ColShape shape, Player player)
        {
            string state = this.State(player);

            NAPI.ClientEvent.TriggerClientEvent(player, "DisplayPressE", true, this.NpcName, state);
        }

        public void OnEntityExitColShape(ColShape shape, Player player)
        {
            string state = this.State(player);

            NAPI.ClientEvent.TriggerClientEvent(player, "DisplayPressE", false, this.NpcName, state);
        }

        private string State(Player player)
        {
            string state = "join";

            if (PlayerUtil.GetById(player.Id).Fraction != null)
            {
                state = "leave";
            }

            return state;
        }
    }
}
