using System.Threading.Tasks;
using Gamemode.Cache.GangWar;
using Gamemode.Jobs.GangWar;
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
			string? initGangWarCronExpression = System.Environment.GetEnvironmentVariable("INIT_GANG_WAR_CRON_EXPRESSION");
			if (initGangWarCronExpression == null)
			{
				initGangWarCronExpression = "0 20,50 9-20 * * ?";
			}
			else
			{
				initGangWarCronExpression = initGangWarCronExpression.Replace("\\", string.Empty);
			}

			string? startGangWarCronExpression = System.Environment.GetEnvironmentVariable("START_GANG_WAR_CRON_EXPRESSION");
			if (startGangWarCronExpression == null)
			{
				startGangWarCronExpression = "0 30,00 9-20 * * ?";
			}
			else
			{
				startGangWarCronExpression = startGangWarCronExpression.Replace("\\", string.Empty);
			}

			string? finishWarCronExpression = System.Environment.GetEnvironmentVariable("FINISH_GANG_WAR_CRON_EXPRESSION");
			if (finishWarCronExpression == null)
			{
				finishWarCronExpression = "0 40,10 9-20 * * ?";
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
