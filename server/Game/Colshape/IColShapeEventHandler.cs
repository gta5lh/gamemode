// <copyright file="IColShapeEventHandler.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Colshape
{
	using GTANetworkAPI;

	public interface IColShapeEventHandler
	{
		void OnEntityEnterColShape(ColShape shape, Player player);

		void OnEntityExitColShape(ColShape shape, Player player);
	}
}
