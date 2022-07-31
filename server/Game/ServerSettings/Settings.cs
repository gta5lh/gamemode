// <copyright file="Settings.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.ServerSettings
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

			Logger.Info($"Settings: {Environment} {ServerID}");
		}

		public static bool IsProduction()
		{
			return Environment == "production";
		}

		public static void Init()
		{
		}

		private static readonly NLog.ILogger Logger = Gamemode.Logger.Logger.LogFactory.GetCurrentClassLogger();
	}
}
