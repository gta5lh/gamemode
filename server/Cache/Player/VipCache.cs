using System.Collections.Generic;
using Gamemode.Utils;
using GTANetworkAPI;

namespace Gamemode.Cache.Player
{
	public class VipsCache
	{
		private static readonly Dictionary<long, string> Vips = new Dictionary<long, string>();

		public static void LoadVipToCache(long staticId, string name)
		{
			if (Vips.ContainsKey(staticId))
			{
				return;
			}

			Vips.Add(staticId, name);
		}

		public static void UnloadVipFromCache(long staticId)
		{
			Vips.Remove(staticId);
		}

		public static string GetVipNames()
		{
			return string.Join(", ", Vips.Values);
		}

		public static void SendMessageToAllVipsChat(string message, bool isPremium)
		{
			string prefix = isPremium ? $"{ChatColor.PremiumChatPrefixColor}[PREMIUM]" : $"{ChatColor.VipChatPrefixColor}VIP";

			foreach (long vipStaticId in Vips.Keys)
			{
				NAPI.Chat.SendChatMessageToPlayer(PlayerUtil.GetByStaticId(vipStaticId), $"{prefix}~w~ {message}");
			}
		}
	}
}
