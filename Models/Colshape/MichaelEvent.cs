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
            NAPI.ClientEvent.TriggerClientEvent(player, "DisplayPressE", true);
        }

        public void OnEntityExitColShape(ColShape shape, Player player)
        {
            NAPI.ClientEvent.TriggerClientEvent(player, "DisplayPressE", false);
        }
    }
}
