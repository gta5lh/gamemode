using System.Collections.Generic;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
using Gamemode.Cache.GangWar;
using Gamemode.Jobs.GangWar;
using Gamemode.Models.Player;
using Gamemode.Services.Player;
using GTANetworkAPI;
using Quartz;
using Quartz.Impl;

namespace Gamemode.Controllers
{
	public class GangWarController : Script
	{
		private static IScheduler scheduler;

		public static async Task StartGangWarJobs()
		{
			string? initGangWarCronExpression = System.Environment.GetEnvironmentVariable("INIT_GANG_WAR_CRON_EXPRESSION");
			if (initGangWarCronExpression == null)
			{
				initGangWarCronExpression = "0 50 8-18 * * ?";
			}
			else
			{
				initGangWarCronExpression = initGangWarCronExpression.Replace("\\", string.Empty);
			}

			string? startGangWarCronExpression = System.Environment.GetEnvironmentVariable("START_GANG_WAR_CRON_EXPRESSION");
			if (startGangWarCronExpression == null)
			{
				startGangWarCronExpression = "0 00 9-19 * * ?";
			}
			else
			{
				startGangWarCronExpression = startGangWarCronExpression.Replace("\\", string.Empty);
			}

			string? finishWarCronExpression = System.Environment.GetEnvironmentVariable("FINISH_GANG_WAR_CRON_EXPRESSION");
			if (finishWarCronExpression == null)
			{
				finishWarCronExpression = "0 15 9-19 * * ?";
			}
			else
			{
				finishWarCronExpression = finishWarCronExpression.Replace("\\", string.Empty);
			}

			StdSchedulerFactory factory = new StdSchedulerFactory();
			scheduler = await factory.GetScheduler();

			await scheduler.Start();

			InitGangWarJob initGangWarJob = new InitGangWarJob(initGangWarCronExpression);
			StartGangWarJob startGangWarJob = new StartGangWarJob(startGangWarCronExpression);
			FinishGangWarJob finishGangWarJob = new FinishGangWarJob(finishWarCronExpression);

			Task initGangWarJobTask = initGangWarJob.Configure(scheduler);
			Task startGangWarJobTask = startGangWarJob.Configure(scheduler);
			Task finishGangWarJobTask = finishGangWarJob.Configure(scheduler);

			Task.WaitAll(startGangWarJobTask, finishGangWarJobTask, initGangWarJobTask);
		}

		public static async Task StopGangWarJobs()
		{
			await scheduler.Shutdown();
		}
	}
}
