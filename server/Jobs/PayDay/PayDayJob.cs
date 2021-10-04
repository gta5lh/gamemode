using System;
using System.Threading.Tasks;
using Gamemode.Models.Player;
using Gamemode.Services.Player;
using GamemodeCommon.Models;
using GTANetworkAPI;
using Quartz;

namespace Gamemode.Jobs.PayDay
{
	public class PayDayJob : IJob
	{
		private static readonly double PaydayInterval30Minutes = 1000 * 60 * 30;
		private static readonly double PaydayAllowedLeeway = -(1000 * 60);
		// private static readonly double PaydayInterval30Minutes = 1000;
		// private static readonly double PaydayAllowedLeeway = 0;

		private string cronExpression;

		public PayDayJob(string cronExpression)
		{
			this.cronExpression = cronExpression;
		}

		public PayDayJob()
		{
		}

		public async Task Configure(IScheduler scheduler)
		{
			IJobDetail job = JobBuilder.Create<PayDayJob>()
				.WithIdentity("pay_day", "pay_day")
				.Build();

			ITrigger trigger = TriggerBuilder.Create()
				.WithIdentity("pay_day", "pay_day")
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
			NAPI.Task.Run(async () =>
			{
				DateTime paydayTime = DateTime.UtcNow.AddMilliseconds(PaydayAllowedLeeway);

				foreach (CustomPlayer player in NAPI.Pools.GetAllPlayers())
				{
					if (player.Fraction == null || player.LoggedInAt == null)
					{
						continue;
					}

					if (DateTime.Compare(player.LoggedInAt.Value.AddMilliseconds(PaydayInterval30Minutes), paydayTime) >= 1)
					{
						continue;
					}

					long salary = GangUtil.SalaryByRank[player.FractionRank.Value] * 100; // TODO: REMOVE ME AFTER OPEN BETA TEST.
					player.Money += salary;
					player.SendNotification($"[Payday] На счет поступило: {salary} $", 0, 5000, NotificationType.Success);
					await ExperienceService.ChangeExperience(player, 3);
				}
			});
		}
	}
}
