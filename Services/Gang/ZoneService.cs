using System;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Services
{
	public static class ZoneService
	{
		public static void StartCapture(int zoneID)
		{
			NAPI.ClientEvent.TriggerClientEventForAll("CaptureStart", zoneID);
		}

		public static void FinishCapture(int zoneID, byte winnerFractionID)
		{
			NAPI.ClientEvent.TriggerClientEventForAll("CaptureFinish", zoneID, GangUtil.BlipColorByGangId[winnerFractionID]);
		}
	}
}
