// <copyright file="IColShapeEnterEvent.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Colshape
{
    using GTANetworkAPI;

    public interface IColShapeEventHandler
    {
        void OnEntityEnterColShape(ColShape shape, Player player);
        void OnEntityExitColShape(ColShape shape, Player player);
    }
}
