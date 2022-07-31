// <copyright file="Settings.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.ServerSettings
{
	public static class Settings
	{
		private static readonly NLog.ILogger Logger = Gamemode.Logger.Logger.LogFactory.GetCurrentClassLogger();

		static Settings()
		{
			if (Environment == null)
			{
				System.Environment.FailFast("Specify GS_ENVIRONMENT");
			}

			if (ServerID == null)
			{
				System.Environment.FailFast("Specify GS_ID");
			}

			if (PlatformURL == null)
			{
				System.Environment.FailFast("Specify GS_PLATFORM_URL");
			}

			if (PlatformCertificate == null)
			{
				System.Environment.FailFast("Specify GS_PLATFORM_CERTIFICATE");
			}

			Logger.Info($"Settings: {Environment} {ServerID}");
		}

#pragma warning disable CS8601
		public static string Environment { get; } = System.Environment.GetEnvironmentVariable("GS_ENVIRONMENT");

		public static string ServerID { get; } = System.Environment.GetEnvironmentVariable("GS_ID");

		public static string PlatformURL { get; } = System.Environment.GetEnvironmentVariable("GS_PLATFORM_URL");

		public static string PlatformCertificate { get; } = System.Environment.GetEnvironmentVariable("GS_PLATFORM_CERTIFICATE");
#pragma warning restore CS8601

		public static bool IsProduction()
		{
			return Environment == "production";
		}

		public static void Init()
		{
			// Method intentionally left empty.
		}
	}
}
