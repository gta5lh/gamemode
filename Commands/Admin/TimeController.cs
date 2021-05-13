using System;
using System.Timers;
using GTANetworkAPI;

namespace Gamemode.Commands.Admin
{
    public class TimeController : Script
    {
        private static Timer TimeSyncTimer;

        private static DateTime Now;
        private static readonly double TimeSyncInterval1Minute = 1000 * 60;

        public static void InitTimeSyncTimer()
        {
            TimeSyncTimer = new Timer(TimeSyncInterval1Minute);
            TimeSyncTimer.Elapsed += OnTimeSync;
            TimeSyncTimer.AutoReset = true;
            TimeSyncTimer.Start();
        }

        public static void SyncTime()
        {
            NAPI.Task.Run(() =>
            {
                Now = DateTime.UtcNow.AddHours(3);
                NAPI.World.SetTime(Now.Hour, Now.Minute, Now.Second);
            });
        }

        private static void OnTimeSync(object sender, ElapsedEventArgs e)
        {
            SyncTime();
        }
    }
}
