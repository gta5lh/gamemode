// <copyright file="CarSelectionEvent.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Colshape
{
    using GTANetworkAPI;

    public class CarSelectionEvent : IColShapeEventHandler
    {
        public void OnEntityEnterColShape(ColShape shape, Player player)
        {
            NAPI.Chat.SendChatMessageToPlayer(player, "car selection");
        }

        public void OnEntityExitColShape(ColShape shape, Player player)
        {
        }
    }
}
