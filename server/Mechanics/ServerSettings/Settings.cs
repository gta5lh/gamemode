
namespace Gamemode.Mechanics.ServerSettings
{
	public static class Settings
	{
		public static readonly string Environment;
		public static readonly string ServerID;

		static Settings()
		{
			string? environment = System.Environment.GetEnvironmentVariable("GS_ENVIRONMENT");
			if (environment == null)
			{
				System.Environment.FailFast("Specify GS_ENVIRONMENT");
			}

			string? serverID = System.Environment.GetEnvironmentVariable("GS_ID");
			if (serverID == null)
			{
				System.Environment.FailFast("Specify GS_ID");
			}

			Environment = environment;
			ServerID = serverID;

			logger.Info($"Settings: {Environment} {ServerID}");
		}

		public static void Init()
		{ }

		private static readonly NLog.ILogger logger = Logger.Logger.LogFactory.GetCurrentClassLogger();
	}
}
