// <copyright file="Weather.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Weather.Controllers
{
	using System.Timers;
	using GTANetworkAPI;

	public class Weather : Script
	{
		private const double WeatherSyncInterval1Minute = 1000 * 60;

		public static void InitWeatherSyncTimer()
		{
			SetWeatherToCurrent();
			Timer weatherSyncTimer = new(WeatherSyncInterval1Minute);
			weatherSyncTimer.Elapsed += OnWeatherSync;
			weatherSyncTimer.AutoReset = true;
			weatherSyncTimer.Start();
		}

		private static void OnWeatherSync(object sender, ElapsedEventArgs e)
		{
			NAPI.Task.Run(() => SetWeatherToCurrent());
		}

		private static void SetWeatherToCurrent()
		{
			NAPI.World.SetWeather(GTANetworkAPI.Weather.EXTRASUNNY);
		}
	}
}
