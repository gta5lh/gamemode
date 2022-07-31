// <copyright file="Time.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace GamemodeClient.Utils
{
	using System;

	internal static class Time
	{
		public static long GetCurTimestamp()
		{
			return new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
		}
	}
}
