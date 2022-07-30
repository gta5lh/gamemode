using System.Collections.Generic;
using Gamemode.Game.Chat;
using GTANetworkAPI;

namespace Gamemode.Game.Vip
{
	public class Cache
	{
		private static readonly NLog.ILogger logger = Logger.Logger.LogFactory.GetCurrentClassLogger();
		private static readonly Dictionary<string, string> Vips = new Dictionary<string, string>();

		public static void LoadVipToCache(string staticId, string name)
		{
			if (Vips.ContainsKey(staticId))
			{
				return;
			}

			Vips.Add(staticId, name);
			logger.Info($"Loaded VIP to cache. name={name}, static_id={staticId}");
		}

		public static void UnloadVipFromCache(string staticId)
		{
			Vips.Remove(staticId);
			logger.Info($"Unloaded VIP from cache. static_id={staticId}");
		}

		public static string GetVipNames()
		{
			return string.Join(", ", Vips.Values);
		}

		public static void SendMessageToAllVipsChat(string message, bool isPremium)
		{
			string prefix = isPremium ? $"{ChatColor.PremiumChatPrefixColor}[PREMIUM]" : $"{ChatColor.VipChatPrefixColor}VIP";

			foreach (string vipStaticId in Vips.Keys)
			{
				NAPI.Chat.SendChatMessageToPlayer(PlayerUtil.GetByStaticId(vipStaticId), $"{prefix}~w~ {message}");
			}
		}
	}
}
