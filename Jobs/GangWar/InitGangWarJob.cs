using System;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
using Gamemode.Cache.GangWar;
using GTANetworkAPI;
using Quartz;

namespace Gamemode.Jobs.GangWar
{
	public class InitGangWarJob : IJob
	{
		private string cronExpression;

		public InitGangWarJob(string cronExpression)
		{
			this.cronExpression = cronExpression;
		}

		public InitGangWarJob()
		{
		}

		public async Task Configure(IScheduler scheduler)
		{
			IJobDetail job = JobBuilder.Create<InitGangWarJob>()
				.WithIdentity("init_gang_war_job", "gang_war")
				.Build();

			ITrigger trigger = TriggerBuilder.Create()
				.WithIdentity("init_gang_war_trigger", "gang_war")
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
			await Services.GangWarService.InitGangWar();
		}
	}
}
