// <copyright file="Cache.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Vip
{
	using System.Collections.Generic;
	using Gamemode.Game.Chat;
	using GTANetworkAPI;

	public static class Cache
	{
		private static readonly NLog.ILogger Logger = Gamemode.Logger.Logger.LogFactory.GetCurrentClassLogger();
		private static readonly Dictionary<string, string> Vips = new();

		public static void LoadVipToCache(string staticId, string name)
		{
			if (Vips.ContainsKey(staticId))
			{
				return;
			}

			Vips.Add(staticId, name);
			Logger.Info($"Loaded VIP to cache. name={name}, static_id={staticId}");
		}

		public static void UnloadVipFromCache(string staticId)
		{
			Vips.Remove(staticId);
			Logger.Info($"Unloaded VIP from cache. static_id={staticId}");
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
				NAPI.Chat.SendChatMessageToPlayer(Gamemode.Game.Player.Util.GetByStaticId(vipStaticId), $"{prefix}~w~ {message}");
			}
		}
	}
}
