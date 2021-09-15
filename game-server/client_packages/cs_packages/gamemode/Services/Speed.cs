namespace GamemodeClient.Services
{
	using System;
	using RAGE.Elements;

	public static class Speed
	{
		public static int GetPlayerRealSpeed(Player player)
		{
			return Convert.ToInt32(player.GetSpeed() * 3.6);
		}
	}
}
