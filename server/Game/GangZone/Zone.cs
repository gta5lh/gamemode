// <copyright file="Zone.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Services
{
	using Gamemode.Game.Gang;
	using GTANetworkAPI;

	public static class Zone
	{
		public static void StartCapture(long zoneID)
		{
			NAPI.ClientEvent.TriggerClientEventForAll("CaptureStart", zoneID);
		}

		public static void FinishCapture(long zoneID, long winnerFractionID)
		{
			NAPI.ClientEvent.TriggerClientEventForAll("CaptureFinish", zoneID, Util.BlipColorByGangId[winnerFractionID]);
		}
	}
}
