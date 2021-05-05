// <copyright file="ItemSelectionEvent.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Colshape
{
    using GTANetworkAPI;

    public class ItemSelectionEvent : IColShapeEventHandler
    {
        public void OnEntityEnterColShape(ColShape shape, Player player)
        {
            NAPI.Chat.SendChatMessageToPlayer(player, "item selection");
        }

        public void OnEntityExitColShape(ColShape shape, Player player)
        {
        }
    }
}
