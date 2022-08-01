// <copyright file="Exception.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Exception
{
	using System.Threading.Tasks;
	using Gamemode.Game.ServerSettings;
	using GTANetworkAPI;
	using Rollbar;

	public class Exception : Script
	{
		public static void InitRollbar()
		{
			if (Settings.RollbarToken == "")
			{
				return;
			}

			RollbarLocator.RollbarInstance.Configure(new RollbarConfig
			{
				AccessToken = Settings.RollbarToken,
				Environment = Settings.Environment,
			});

			Logger.Logger.BaseLogger.Info($"Successfully inited Rollbar on {Settings.Environment}");

			TaskScheduler.UnobservedTaskException += (sender, args) =>
			{
				RollbarLocator.RollbarInstance.Error(args.Exception);
			};
		}

		[ServerEvent(Event.FirstChanceException)]
		private void OnFirstChanceException(System.Exception exception)
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
