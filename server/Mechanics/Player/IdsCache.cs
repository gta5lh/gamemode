using System;
using System.Collections.Concurrent;

namespace Gamemode.Mechanics.Player
{
	public static class IdsCache
	{
		private static readonly NLog.ILogger logger = Logger.Logger.LogFactory.GetCurrentClassLogger();
		private static ConcurrentDictionary<ushort, string> dynamicStaticPairs = new ConcurrentDictionary<ushort, string>();
		private static ConcurrentDictionary<string, ushort> staticDynamicPairs = new ConcurrentDictionary<string, ushort>();

		public static ushort? DynamicIdByStatic(string id)
		{
			return staticDynamicPairs.TryGetValue(id, out ushort dynamicId) ? (ushort?)dynamicId : null;
		}

		public static string? StaticIdByDynamic(ushort id)
		{
			return dynamicStaticPairs.TryGetValue(id, out string? staticId) ? (string?)staticId : null;
		}

		public static string? StaticIdByDynamic(string id)
		{
			try
			{
				return dynamicStaticPairs.TryGetValue(ushort.Parse(id), out string? staticId) ? (string?)staticId : null;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static void LoadIdsToCache(ushort dynamicId, string staticId)
		{
			dynamicStaticPairs[dynamicId] = staticId;
			staticDynamicPairs[staticId] = dynamicId;
			logger.Info($"Loaded player ids to cache. dynamic_id={dynamicId}, static_id={staticId}");
		}

		public static void UnloadIdsFromCacheByDynamicId(ushort id)
		{
			if (dynamicStaticPairs.TryRemove(id, out string? staticId))
			{
				staticDynamicPairs.TryRemove(staticId, out _);
				logger.Info($"Unloaded player ids from cache. dynamic_id={id}");
			}
		}
	}
}
