using System;
using System.Collections.Generic;
using System.Timers;
using GTANetworkAPI;

namespace Gamemode.Mechanics.Time.Controllers
{
	public class Time : Script
	{
		private static DummyEntity SyncDummyEntity;

		private static Timer TimeSyncTimer;

		public static DateTime CurrentDateTime { get; private set; }
		public static TimeSpan CurrentTime { get; private set; }
		private static readonly double TimeSyncInterval1Minute = 1000 * 60;

		public static void InitTimeSyncTimer()
		{
			SyncDummyEntity = NAPI.DummyEntity.CreateDummyEntity(0, new Dictionary<string, object>());
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
			SyncTime();
		}

		private static void SetTimeToCurrent()
		{
			CurrentDateTime = DateTime.UtcNow.AddHours(3);
			CurrentTime = CurrentDateTime.TimeOfDay;
			NAPI.World.SetTime(CurrentTime.Hours, CurrentTime.Minutes, CurrentTime.Seconds);
			SyncTime();
		}

		private static void SyncTime()
		{
			SyncDummyEntity.SetSharedData(GamemodeCommon.Models.Data.DataKey.CurrentTime, new Dictionary<string, object>(){
				{"hours", CurrentTime.Hours},
				{"minutes", CurrentTime.Minutes},
				{"day", CurrentDateTime.Day},
				{"month", CurrentDateTime.Month},
			});
		}
	}
}
