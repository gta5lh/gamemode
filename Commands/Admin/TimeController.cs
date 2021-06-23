using System;
using System.Timers;
using GTANetworkAPI;

namespace Gamemode.Commands.Admin
{
	public class TimeController : Script
	{
		private static Timer TimeSyncTimer;

		public static TimeSpan CurrentTime { get; private set; }
		private static readonly double TimeSyncInterval1Minute = 1000 * 60;

		public static void InitTimeSyncTimer()
		{
			SetTimeToCurrent();
			TimeSyncTimer = new Timer(TimeSyncInterval1Minute);
			TimeSyncTimer.Elapsed += OnTimeSync;
			TimeSyncTimer.AutoReset = true;
			TimeSyncTimer.Start();
		}

		private static void OnTimeSync(object sender, ElapsedEventArgs e)
		{
			NAPI.Task.Run(() =>
			{
				SetTimeToCurrent();
			});
		}

		public static void StopTimeSync()
		{
			TimeSyncTimer.Stop();
		}

		public static void StartTimeSync()
		{
			SetTimeToCurrent();
			TimeSyncTimer.Start();
		}

		public static void SetCurrentTime(TimeSpan time)
		{
			CurrentTime = time;
			NAPI.World.SetTime(CurrentTime.Hours, CurrentTime.Minutes, CurrentTime.Seconds);
		}

		private static void SetTimeToCurrent()
		{
			CurrentTime = DateTime.UtcNow.AddHours(3).TimeOfDay;
			NAPI.World.SetTime(CurrentTime.Hours, CurrentTime.Minutes, CurrentTime.Seconds);
		}
	}
}
