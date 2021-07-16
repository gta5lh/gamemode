using Gamemode.Models.Gangs;
using Gamemode.Models.Player;
using Gamemode.Models.Spawn;
using Gamemode.Models.Vehicle;
using GamemodeCommon.Models.Data;
using GTANetworkAPI;
using Newtonsoft.Json;

namespace Gamemode.Controllers
{
	public class NpcController : Script
	{
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
			Color gangColor = GangUtil.GangColorByName[playerSelectedGangRequest.Gang];

			if (player.OneTimeVehicleId != null)
			{
				VehicleUtil.GetById(player.OneTimeVehicleId.Value).Delete();
			}

			CustomVehicle vehicle = (CustomVehicle)NAPI.Vehicle.CreateVehicle(VehicleHash.Ruiner3, vehicleSpawnLocation.Position, vehicleSpawnLocation.Heading, 0, 0, "NEWBIE");
			vehicle.OwnerPlayerId = player.Id;
			vehicle.CustomPrimaryColor = gangColor;
			vehicle.CustomSecondaryColor = gangColor;
			vehicle.Rotation = new Vector3(0, 0, vehicleSpawnLocation.Heading);

			player.SetIntoVehicle(vehicle, 0);
			player.OneTimeVehicleId = vehicle.Id;
			vehicle.SetSharedData(DataKey.VehicleCollisionDisabled, true);

			NAPI.ClientEvent.TriggerClientEvent(player, "CreateWaypoint", markerLocation.Position.X, markerLocation.Position.Y);
		}
	}

	public class PlayerSelectedGangRequest
	{
		public string Gang { get; set; }

		public string Npc { get; set; }
	}
}
