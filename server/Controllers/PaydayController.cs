namespace Gamemode.Controllers
{
	using System.Threading.Tasks;
	using Gamemode.Jobs.PayDay;
	using GTANetworkAPI;
	using Quartz;
	using Quartz.Impl;

	public class PayDayController : Script
	{
		private static IScheduler scheduler;

		public static async Task StartPayDayJob()
		{
			StdSchedulerFactory factory = new StdSchedulerFactory();
			scheduler = await factory.GetScheduler();

			await scheduler.Start();

			string payDayCronExpression = "0 0,30 * * * ?";
			PayDayJob payDayJob = new PayDayJob(payDayCronExpression);

			await payDayJob.Configure(scheduler);
		}

		public static async Task StopPayDayJob()
		{
			await scheduler.Shutdown();
		}
	}
}
