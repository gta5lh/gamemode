// <copyright file="PlayerUtil.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using GTANetworkAPI;

    public static class VehicleUtil
    {
        public static Vehicle GetById(ushort vehicleId)
        {
            return NAPI.Entity.GetEntityFromHandle<Vehicle>(new NetHandle(vehicleId, EntityType.Vehicle));
        }
    }
}
