using System;
using System.Threading;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
using Gamemode.Cache.GangWar;
using Gamemode.Cache.GangZone;
using Gamemode.Logger;
using Gamemode.Services;
using GTANetworkAPI;
using Quartz;

namespace Gamemode.Jobs.GangWar
{
	public class FinishGangWarJob : IJob
	{
		private string cronExpression;

		public FinishGangWarJob(string cronExpression)
		{
			this.cronExpression = cronExpression;
		}

		public FinishGangWarJob()
		{
		}

		public async Task Configure(IScheduler scheduler)
		{
			IJobDetail job = JobBuilder.Create<FinishGangWarJob>()
				.WithIdentity("finish_gang_war_job", "gang_war")
				.Build();

			ITrigger trigger = TriggerBuilder.Create()
				.WithIdentity("finish_gang_war_trigger", "gang_war")
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
			await Services.GangWarService.FinishGangWar();
		}
	}
}
