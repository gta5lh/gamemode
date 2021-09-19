using System.Timers;
using GTANetworkAPI;

namespace Gamemode.Commands.Admin
{
	public class WeatherController : Script
	{
		private static Timer WeatherSyncTimer;
		private static readonly double WeatherSyncInterval1Minute = 1000 * 60;

		public static void InitWeatherSyncTimer()
		{
			SetWeatherToCurrent();
			WeatherSyncTimer = new Timer(WeatherSyncInterval1Minute);
			WeatherSyncTimer.Elapsed += OnWeatherSync;
			WeatherSyncTimer.AutoReset = true;
			WeatherSyncTimer.Start();
		}

		private static void OnWeatherSync(object sender, ElapsedEventArgs e)
		{
			NAPI.Task.Run(() =>
			{
				SetWeatherToCurrent();
			});
		}

		private static void SetWeatherToCurrent()
		{
			NAPI.World.SetWeather(Weather.EXTRASUNNY);
		}
	}
}
