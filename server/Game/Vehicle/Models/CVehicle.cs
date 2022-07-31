// <copyright file="CVehicle.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Vehicle.Models
{
	using GTANetworkAPI;

	public class CVehicle : Vehicle
	{
		public CVehicle(NetHandle handle)
			: base(handle)
		{
		}

		public ushort OwnerPlayerId { get; set; }
	}
}
