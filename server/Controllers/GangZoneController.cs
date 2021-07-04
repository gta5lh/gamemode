using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
using Gamemode.Cache.GangZone;
using GTANetworkAPI;

namespace Gamemode.Controllers
{
	public class GangZoneController : Script
	{
		[ServerEvent(Event.PlayerConnected)]
		public void OnPlayerConnected(Player player)
		{
			NAPI.ClientEvent.TriggerClientEvent(player, "RenderGangZones", GangZoneCache.Zones);
		}

		private static bool IsInSquare(Vector3 playerPos, Vector3 blipPos)
		{
			Vector3 newPos = playerPos - blipPos;
			if (Math.Abs(newPos.X) < 100 && Math.Abs(newPos.Y) < 100) return true;
			else return false;
		}
	}
}
