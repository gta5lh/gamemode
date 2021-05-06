using System;
using Gamemode.Models.Gangs;
using Gamemode.Models.Player;
using Gamemode.Models.Spawn;
using GTANetworkAPI;
using Newtonsoft.Json;

namespace Gamemode.Controllers
{
    public class NpcController : Script
    {
        private readonly Random random = new Random();

        [RemoteEvent("PlayerSelectedGang")]
        private void OnPlayerSelectedGang(CustomPlayer player, string request)
        {
            if (player.Fraction != null)
            {
                NAPI.ClientEvent.TriggerClientEvent(player, "CloseNpcMenu");
                NAPI.ClientEvent.TriggerClientEvent(player, "DisplayPressE", false);
                return;
            }

            PlayerSelectedGangRequest playerSelectedGangRequest = JsonConvert.DeserializeObject<PlayerSelectedGangRequest>(request);

            Spawn markerLocation = GangSpawns.SpawnByGangName[playerSelectedGangRequest.Gang];
            Spawn vehicleSpawnLocation = PlayerSpawns.VehicleSpawnByNpcName[playerSelectedGangRequest.Npc];

            if (player.SpawnNpcVehicleId != null)
            {
                VehicleUtil.GetById(player.SpawnNpcVehicleId.Value).Delete();
            }

            Color randomColor = new Color(this.random.Next(0, 255), this.random.Next(0, 255), this.random.Next(0, 255));
            Vehicle vehicle = NAPI.Vehicle.CreateVehicle(VehicleHash.Voodoo2, vehicleSpawnLocation.Position, vehicleSpawnLocation.Heading, randomColor.ToInt32(), randomColor.ToInt32(), "NEWBIE");
            player.SetIntoVehicle(vehicle, 0);
            player.SpawnNpcVehicleId = vehicle.Id;

            NAPI.ClientEvent.TriggerClientEvent(player, "CreateWaypoint", markerLocation.Position.X, markerLocation.Position.Y);
        }

        [ServerEvent(Event.PlayerDisconnected)]
        private void OnPlayerDisconnected(CustomPlayer player, DisconnectionType type, string reason)
        {
            if (player.SpawnNpcVehicleId == null)
            {
                return;
            }

            VehicleUtil.GetById(player.SpawnNpcVehicleId.Value).Delete();
        }
    }

    public class PlayerSelectedGangRequest
    {
        public string Gang { get; set; }

        public string Npc { get; set; }
    }
}
