using System;
using RAGE.Elements;

namespace GamemodeClient.Services
{
	public static class Speed
	{
		public static int GetPlayerRealSpeed(Player player)
		{
			return Convert.ToInt32(player.GetSpeed() * 3.6);
		}
	}
}
