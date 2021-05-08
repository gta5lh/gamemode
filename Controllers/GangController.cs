﻿// <copyright file="GangController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
    using Gamemode.Models.Player;
    using Gamemode.Models.Spawn;
    using Gamemode.Models.Vehicle;
    using GTANetworkAPI;
    using Newtonsoft.Json;

    public class GangController : Script
    {
        private readonly Ballas ballas;
        private readonly Bloods bloods;
        private readonly Marabunta marabunta;
        private readonly TheFamilies theFamilies;
        private readonly Vagos vagos;

        public GangController()
        {
            this.ballas = new Ballas();
            this.bloods = new Bloods();
            this.marabunta = new Marabunta();
            this.theFamilies = new TheFamilies();
            this.vagos = new Vagos();
        }

        [ServerEvent(Event.ResourceStartEx)]
        private void ResourceStartEx(string resourceName)
        {
            this.ballas.Create();
            this.bloods.Create();
            this.marabunta.Create();
            this.theFamilies.Create();
            this.vagos.Create();
        }

        [RemoteEvent("PlayerSelectedGangCar")]
        private void OnPlayerSelectedGangCar(CustomPlayer player, string request)
        {
            PlayerSelectedGangCarRequest playerSelectedGangCarRequest = JsonConvert.DeserializeObject<PlayerSelectedGangCarRequest>(request);

            string gangName = GangUtil.GangNameById[player.Fraction.Value];
            Spawn carSpawn = GangUtil.CarSpawnByGangId[player.Fraction.Value];
            Color gangColor = GangUtil.GangColorByName[gangName];
            uint vehicleHash = NAPI.Util.GetHashKey(playerSelectedGangCarRequest.CarName);

            if (player.OneTimeVehicleId != null)
            {
                VehicleUtil.GetById(player.OneTimeVehicleId.Value).Delete();
            }

            CustomVehicle vehicle = (CustomVehicle)NAPI.Vehicle.CreateVehicle(vehicleHash, carSpawn.Position, carSpawn.Heading, 0, 0, gangName);
            vehicle.OwnerPlayerId = player.Id;
            vehicle.CustomPrimaryColor = gangColor;
            vehicle.CustomSecondaryColor = gangColor;
            vehicle.Rotation = new Vector3(0, 0, carSpawn.Heading);

            player.SetIntoVehicle(vehicle, 0);
            player.OneTimeVehicleId = vehicle.Id;
        }
    }

    public class PlayerSelectedGangCarRequest
    {
        public string CarName { get; set; }
    }
}
