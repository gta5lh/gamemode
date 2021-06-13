using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Timers;
using Gamemode.ApiClient.Models;
using Gamemode.Cache.GangWar;
using Gamemode.Jobs;
using Gamemode.Models.Player;
using GTANetworkAPI;
using Quartz;
using Quartz.Impl;

namespace Gamemode.Controllers
{
    public class GangWarController : Script
    {
        private static IScheduler scheduler;

        public static async void StartGangWarJobs()
        {
            StdSchedulerFactory factory = new StdSchedulerFactory();
            scheduler = await factory.GetScheduler();

            await scheduler.Start();

            string? startGangWarCronExpression = System.Environment.GetEnvironmentVariable("START_GANG_WAR_CRON_EXPRESSION");
            startGangWarCronExpression = startGangWarCronExpression.Replace("\\", string.Empty);
            if (startGangWarCronExpression == null)
            {
                startGangWarCronExpression = "0 20,50 9-20 * * ?";
            }

            StartGangWarJob startGangWarJob = new StartGangWarJob(startGangWarCronExpression);
            await startGangWarJob.Configure(scheduler);

            string? gangWarCronExpression = System.Environment.GetEnvironmentVariable("GANG_WAR_CRON_EXPRESSION");
            gangWarCronExpression = startGangWarCronExpression.Replace("\\", string.Empty);
            if (gangWarCronExpression == null)
            {
                gangWarCronExpression = "0 20,50 9-20 * * ?";
            }

            GangWarJob gangWarJob = new GangWarJob(gangWarCronExpression);
            await gangWarJob.Configure(scheduler);
        }

        public static async Task StopGangWarJobs()
        {
            await scheduler.Shutdown();
        }

        [ServerEvent(Event.PlayerDeath)]
        private async void OnPlayerDeath(CustomPlayer target, CustomPlayer killer, uint reason)
        {
            if (killer == null)
            {
                return;
            }

            if (target.Fraction == null || killer.Fraction == null)
            {
                return;
            }

            if (!target.IsInWarZone || !killer.IsInWarZone)
            {
                return;
            }

            GangWarCache.AddKill(killer.Fraction.Value);
            killer.SendChatMessage(GangWarCache.GetKillsMessage());
        }
    }
}
