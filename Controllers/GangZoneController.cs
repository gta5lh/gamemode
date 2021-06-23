using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
using GTANetworkAPI;

namespace Gamemode.Controllers
{
	public class GangZoneController : Script
	{
		private static List<Zone> Zones;

		[ServerEvent(Event.PlayerConnected)]
		public void OnPlayerConnected(Player player)
		{
			NAPI.ClientEvent.TriggerClientEvent(player, "RenderGangZones", Zones);
		}

		public static void OnCaptureStart(int blipId)
		{
			//Colors[blipId] *= -1;
			//NAPI.ClientEvent.TriggerClientEventForAll("CaptureStart", blipId);
		}

		public static void OnCaptureEnd(int blipId, int color)
		{
			//Colors[blipId] = color;
			//NAPI.ClientEvent.TriggerClientEventForAll("CaptureEnd", blipId, color);
		}

		public static int TryCaptureStart(Player player)
		{
			//for (int i = 0; i < BlipsPos.Length; i++)
			//{
			//    if (!IsInSquare(player.Position, BlipsPos[i])) continue;
			//    OnCaptureStart(i);
			//    return i;
			//}
			return -1;
		}

		private static bool IsInSquare(Vector3 playerPos, Vector3 blipPos)
		{
			Vector3 newPos = playerPos - blipPos;
			if (Math.Abs(newPos.X) < 100 && Math.Abs(newPos.Y) < 100) return true;
			else return false;
		}

		public static async void InitGangZones()
		{
			Zones = await LoadZones();
		}

		public static async Task<List<Zone>> LoadZones()
		{
			List<Zone> zones;

			try
			{
				zones = await ApiClient.ApiClient.AllZones();
			}
			catch (Exception e)
			{
				return null;
			}

			for (int i = 0; i < zones.Count; i++)
			{
				zones[i].BlipColor = GangUtil.BlipColorByGangId[zones[i].FractionId];
			}

			return zones;
		}
	}
}
