// <copyright file="VehicleUtil.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Vehicle
{
	using GTANetworkAPI;

	public static class VehicleUtil
	{
		public static Vehicle GetById(ushort vehicleId)
		{
			return NAPI.Entity.GetEntityFromHandle<Vehicle>(new NetHandle(vehicleId, EntityType.Vehicle));
		}

		public static string DisplayName(Vehicle vehicle, string defaultName)
		{
			return vehicle.DisplayName ?? defaultName;
		}
	}
}
