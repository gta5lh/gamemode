// <copyright file="Time.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Time.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Timers;
	using GTANetworkAPI;

	public class Time : Script
	{
		private static DummyEntity syncDummyEntity;

		private static Timer timeSyncTimer;

		public static DateTime CurrentDateTime { get; private set; }

		public static TimeSpan CurrentTime { get; private set; }

		private const double TimeSyncInterval1Minute = 1000 * 60;

		public static void InitTimeSyncTimer()
		{
			syncDummyEntity = NAPI.DummyEntity.CreateDummyEntity(0, new Dictionary<string, object>());
			SetTimeToCurrent();
			timeSyncTimer = new Timer(TimeSyncInterval1Minute);
			timeSyncTimer.Elapsed += OnTimeSync;
			timeSyncTimer.AutoReset = true;
			timeSyncTimer.Start();
		}

		private static void OnTimeSync(object sender, ElapsedEventArgs e)
		{
			NAPI.Task.Run(() => SetTimeToCurrent());
		}

		public static void StopTimeSync()
		{
			timeSyncTimer.Stop();
		}

		public static void StartTimeSync()
		{
			SetTimeToCurrent();
			timeSyncTimer.Start();
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
			syncDummyEntity.SetSharedData(GamemodeCommon.Models.Data.DataKey.CurrentTime, new Dictionary<string, object>()
			{
				{ "hours", CurrentTime.Hours },
				{ "minutes", CurrentTime.Minutes },
				{ "day", CurrentDateTime.Day },
				{ "month", CurrentDateTime.Month },
			});
		}
	}
}
