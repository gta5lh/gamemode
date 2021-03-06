// <copyright file="IColShapeEnterEvent.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using GTANetworkAPI;

    public interface IColShapeEnterEvent
    {
        void OnEntityEnterColShape(ColShape shape, Player player);
    }
}
