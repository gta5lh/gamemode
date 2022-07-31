// <copyright file="CVehicle.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Vehicle.Models
{
	using GTANetworkAPI;

	public class CVehicle : GTANetworkAPI.Vehicle
	{
		public ushort OwnerPlayerId { get; set; }

		public CVehicle(NetHandle handle)
			: base(handle)
		{
		}
	}
}
