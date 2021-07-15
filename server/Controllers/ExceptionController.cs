using System;
using System.Threading.Tasks;
using GTANetworkAPI;
using Rollbar;

namespace Gamemode.Controllers
{
	public class ExceptionController : Script
	{
		public static void InitRollbar()
		{
			string? rollbarToken = System.Environment.GetEnvironmentVariable("ROLLBAR_TOKEN");
			if (rollbarToken == null)
			{
				return;
			}

			string? environment = System.Environment.GetEnvironmentVariable("ENVIRONMENT");
			if (environment == null)
			{
				environment = "development";
			}

			RollbarLocator.RollbarInstance.Configure(new RollbarConfig
			{
				AccessToken = rollbarToken,
				Environment = environment
			});

			Logger.Logger.BaseLogger.Info($"Successfully inited Rollbar on {environment}");

			TaskScheduler.UnobservedTaskException += (sender, args) =>
			{
				RollbarLocator.RollbarInstance.Error(args.Exception);
			};
		}

		[ServerEvent(Event.FirstChanceException)]
		private void OnFirstChanceException(Exception exception)
		{
			if (
				exception.Message == "Cannot access a disposed object.\nObject name: 'SslStream'." ||
				exception.Message == "Unable to read data from the transport connection: Operation canceled." ||
				exception.Message == "Input string was not in a correct format." ||
				exception.Message.Contains("StatusCode=")
			) return;

			RollbarLocator.RollbarInstance.Error(exception);
		}

		[ServerEvent(Event.UnhandledException)]
		private void OnUnhandledException(Exception exception)
		{
			RollbarLocator.RollbarInstance.Error(exception);
		}
	}
}
