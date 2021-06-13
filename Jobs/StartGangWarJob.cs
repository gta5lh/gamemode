using System;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
using Gamemode.Cache.GangWar;
using GTANetworkAPI;
using Quartz;

namespace Gamemode.Jobs
{
    public class StartGangWarJob : IJob
    {
        const string startGangWarChatMessage1 = "Начинается война за территорию через 10 минут";
        const string startGangWarChatMessage2 = "• Нападение на {0}";
        const string startGangWarChatMessage3 = "• Координаты: X={0}, Y={1}";

        private string cronExpression;

        public StartGangWarJob(string cronExpression)
        {
            this.cronExpression = cronExpression;
        }

        public StartGangWarJob()
        {
        }

        public async Task Configure(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<StartGangWarJob>()
                .WithIdentity("start_gang_war_job", "gang_war")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("start_gang_war_trigger", "gang_war")
                .StartNow()
                .WithSchedule(CronScheduleBuilder
                    .CronSchedule(this.cronExpression)
                    .InTimeZone(TimeZoneInfo.Utc)
                )
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if (GangWarCache.IsInProgress()) return;

            GangWar gangWar = await ApiClient.ApiClient.StartGangWar();
            GangWarCache.InitGangWarCache(gangWar);

            NAPI.Task.Run(() =>
            {
                NAPI.Chat.SendChatMessageToAll(startGangWarChatMessage1);
                NAPI.Chat.SendChatMessageToAll(String.Format(startGangWarChatMessage2, gangWar.TargetFractionName));
                NAPI.Chat.SendChatMessageToAll(String.Format(startGangWarChatMessage3, gangWar.XCoordinate, gangWar.YCoordinate));
            });
        }
    }
}
