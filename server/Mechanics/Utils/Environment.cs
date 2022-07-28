// <copyright file="Environment.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Mechanics.Utils
{
	public static class Environment
	{
		public static bool IsProduction()
		{
			string? environment = System.Environment.GetEnvironmentVariable("ENVIRONMENT");
			if (environment == null)
			{
				environment = "development";
			}

			return environment == "production";
		}

	}
}
