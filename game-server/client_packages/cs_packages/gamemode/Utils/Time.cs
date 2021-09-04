using System;

namespace GamemodeClient.Utils
{
	class Time
	{
		public static long GetCurTimestamp()
		{
			return new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
		}
	}
}
