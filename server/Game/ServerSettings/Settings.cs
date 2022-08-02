// <copyright file="Settings.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.ServerSettings
{
	public static class Settings
	{
		private static readonly NLog.ILogger Logger = Gamemode.Logger.Logger.LogFactory.GetCurrentClassLogger();

		private static readonly string? EnvironmentValue = System.Environment.GetEnvironmentVariable("GS_ENVIRONMENT");
		private static readonly string? ServerIDValue = System.Environment.GetEnvironmentVariable("GS_ID");
		private static readonly string? PlatformURLValue = System.Environment.GetEnvironmentVariable("GS_PLATFORM_URL");
		private static readonly string? PlatformCertificateValue = System.Environment.GetEnvironmentVariable("GS_PLATFORM_CERTIFICATE");
		private static readonly string? RollbarTokenValue = System.Environment.GetEnvironmentVariable("GS_ROLLBAR_TOKEN");

		static Settings()
		{
			if (EnvironmentValue == null)
			{
				System.Environment.FailFast("Specify GS_ENVIRONMENT");
				return;
			}

			if (ServerIDValue == null)
			{
				System.Environment.FailFast("Specify GS_ID");
				return;
			}

			if (PlatformURLValue == null)
			{
				System.Environment.FailFast("Specify GS_PLATFORM_URL");
				return;
			}

			if (PlatformCertificateValue == null)
			{
				System.Environment.FailFast("Specify GS_PLATFORM_CERTIFICATE");
				return;
			}

			if (RollbarTokenValue == null)
			{
				System.Environment.FailFast("Specify GS_ROLLBAR_TOKEN");
				return;
			}

			Logger.Info($"Settings: {Environment} {ServerID} {PlatformURLValue} {PlatformCertificateValue}");
		}

		public static string Environment { get => EnvironmentValue!; }

		public static string ServerID { get => ServerIDValue!; }

		public static string PlatformURL { get => PlatformURLValue!; }

		public static string PlatformCertificate { get => PlatformCertificateValue!; }

		public static string RollbarToken { get => RollbarTokenValue!; }

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
