// <copyright file="CarSelectionEvent.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Colshape
{
    using GTANetworkAPI;

    public class MichaelEvent : IColShapeEventHandler
    {
        public void OnEntityEnterColShape(ColShape shape, Player player)
        {

            if (PlayerUtil.GetById(player.Id).Fraction != null)
            {
                return;
            }

            NAPI.ClientEvent.TriggerClientEvent(player, "DisplayPressE", true);
        }

        public void OnEntityExitColShape(ColShape shape, Player player)
        {
            if (PlayerUtil.GetById(player.Id).Fraction != null)
            {
                return;
            }

            NAPI.ClientEvent.TriggerClientEvent(player, "DisplayPressE", false);
        }
    }
}
