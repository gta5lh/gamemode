using Gamemode.Models.Gangs;
using Gamemode.Models.Player;
using Gamemode.Models.Spawn;
using GTANetworkAPI;
using System.Collections.Generic;

namespace Gamemode.Controllers
{
	public class SafeZoneController : Script
	{
		public static List<ColShape> ColShapes { get; private set; }

		public SafeZoneController()
		{
			ColShapes = new List<ColShape>();

			foreach (Spawn spawn in PlayerSpawns.SpawnPositions)
			{
				AddSafeZone(spawn.Position);
			}

			foreach (Spawn spawn in GangSpawns.Spawns.Values)
			{
				AddSafeZone(spawn.Position);
			}
		}

		private void AddSafeZone(Vector3 vec)
		{
			ColShapes.Add(NAPI.ColShape.Create2DColShape(vec.X, vec.Y, 100f, 100f));
		}

		[ServerEvent(Event.PlayerEnterColshape)]
		public void OnPlayerEnterColshape(ColShape shape, CustomPlayer player)
		{
			if (!ColShapes.Contains(shape)) return;
			player.TriggerEvent("safeZone", true);
		}

		[ServerEvent(Event.PlayerExitColshape)]
		public void OnPlayerExitColshape(ColShape shape, CustomPlayer player)
		{
			if (!ColShapes.Contains(shape)) return;
			player.TriggerEvent("safeZone", false);

			if (player.Vehicle != null && player.Vehicle.Exists) player.Vehicle.ResetSharedData("vehicle_collision_disabled");
		}
	}
}
