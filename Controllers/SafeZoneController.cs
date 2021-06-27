using Gamemode.Models.Gangs;
using Gamemode.Models.Player;
using Gamemode.Models.Spawn;
using GTANetworkAPI;
using System.Collections.Generic;

namespace Gamemode.Controllers
{
	public class SafeZoneController : Script
	{
		private List<ColShape> ColShapes = new List<ColShape>();

		public SafeZoneController()
		{
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
			ColShapes.Add(NAPI.ColShape.CreateCylinderColShape(vec, 50, 50, 0));
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
		}
	}
}
