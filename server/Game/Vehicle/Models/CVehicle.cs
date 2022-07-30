// <copyright file="PlayerCache.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Game.Vehicle.Models
{
	using GTANetworkAPI;

	public class CVehicle : GTANetworkAPI.Vehicle
	{
		public ushort OwnerPlayerId { get; set; }

		public CVehicle(NetHandle handle) : base(handle)
		{
		}
	}
}
