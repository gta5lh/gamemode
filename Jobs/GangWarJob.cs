using System;
using System.Threading.Tasks;
using Gamemode.Cache.GangWar;
using Gamemode.Cache.GangZone;
using Gamemode.Colshape;
using Gamemode.Services;
using GTANetworkAPI;
using Quartz;

namespace Gamemode.Jobs
{
    public class GangWarJob : IJob
    {
        const string startGangWarChatMessage1 = "Началась война за территорию!";
        const string startGangWarChatMessage2 = "• Нападение на {0}";
        const string startGangWarChatMessage3 = "• Координаты: X={0}, Y={1}";

        private string cronExpression;

        public GangWarJob(string cronExpression)
        {
            this.cronExpression = cronExpression;
        }

        public GangWarJob()
        {
        }

        public async Task Configure(IScheduler scheduler)
        {
            IJobDetail job = JobBuilder.Create<GangWarJob>()
                .WithIdentity("gang_war_job", "gang_war")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("gang_war_trigger", "gang_war")
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
            if (!GangWarCache.IsInited() || GangWarCache.IsInProgress()) return;
            GangWarCache.SetAsInProgress();
            GangZoneCache.MarkAsWarInProgress(GangWarCache.GangWar.ZoneID);

            NAPI.Task.Run(() =>
            {
                NAPI.Chat.SendChatMessageToAll(startGangWarChatMessage1);
                NAPI.Chat.SendChatMessageToAll(String.Format(startGangWarChatMessage2, GangWarCache.GangWar.TargetFractionName));
                NAPI.Chat.SendChatMessageToAll(String.Format(startGangWarChatMessage3, GangWarCache.GangWar.XCoordinate, GangWarCache.GangWar.YCoordinate));

                ColShape colShape = NAPI.ColShape.Create2DColShape(GangWarCache.GangWar.XCoordinate, GangWarCache.GangWar.YCoordinate, 100f, 100f);
                GangWarEvent gangWarEvent = new Colshape.GangWarEvent();
                colShape.OnEntityEnterColShape += gangWarEvent.OnEntityEnterColShape;
                colShape.OnEntityExitColShape += gangWarEvent.OnEntityExitColShape;

                ZoneService.StartCapture(GangWarCache.GangWar.ZoneID);
            });
        }
    }
}
