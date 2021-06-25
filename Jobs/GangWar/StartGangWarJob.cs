using System;
using System.Threading.Tasks;
using Gamemode.Cache.GangWar;
using Gamemode.Cache.GangZone;
using Gamemode.Colshape;
using Gamemode.Services;
using GTANetworkAPI;
using Quartz;

namespace Gamemode.Jobs.GangWar
{
	public class StartGangWarJob : IJob
	{
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
			DateTime finishTime = DateTime.UtcNow.AddMinutes(15);

			if (context.ScheduledFireTimeUtc == null)
			{
				Logger.Logger.BaseLogger.Error("StartGangWarJob null ScheduledFireTimeUtc");
			}
			else
			{
				finishTime = context.ScheduledFireTimeUtc.Value.UtcDateTime.AddMinutes(15);
			}

			await Services.GangWarService.StartGangWar(finishTime);
		}
	}
}
