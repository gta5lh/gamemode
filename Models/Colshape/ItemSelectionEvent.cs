// <copyright file="ItemSelectionEvent.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using GTANetworkAPI;

    public class ItemSelectionEvent : IColShapeEnterEvent
    {
        public void OnEntityEnterColShape(ColShape shape, Player player)
        {
            NAPI.Chat.SendChatMessageToPlayer(player, "item selection");
        }
    }
}
