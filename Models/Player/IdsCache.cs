﻿using System.Collections.Concurrent;

namespace Gamemode.Models.Player
{
    public static class IdsCache
    {
        private static readonly NLog.ILogger logger = Logger.Logger.LogFactory.GetCurrentClassLogger();
        private static ConcurrentDictionary<ushort, long> dynamicStaticPairs = new ConcurrentDictionary<ushort, long>();
        private static ConcurrentDictionary<long, ushort> staticDynamicPairs = new ConcurrentDictionary<long, ushort>();

        public static ushort? DynamicIdByStatic(long id)
        {
            return staticDynamicPairs.TryGetValue(id, out ushort dynamicId) ? (ushort?)dynamicId : null;
        }

        public static ushort? DynamicIdByStatic(string id)
        {
            return staticDynamicPairs.TryGetValue(long.Parse(id), out ushort dynamicId) ? (ushort?)dynamicId : null;
        }

        public static long? StaticIdByDynamic(ushort id)
        {
            return dynamicStaticPairs.TryGetValue(id, out long staticId) ? (long?)staticId : null;
        }

        public static long? StaticIdByDynamic(string id)
        {
            return dynamicStaticPairs.TryGetValue(ushort.Parse(id), out long staticId) ? (long?)staticId : null;
        }

        public static void LoadIdsToCache(ushort dynamicId, long staticId)
        {
            dynamicStaticPairs[dynamicId] = staticId;
            staticDynamicPairs[staticId] = dynamicId;
            logger.Debug("Loaded player ids to cache. dynamic_id={dynamicId}, static_id={staticId}", dynamicId, staticId);
        }

        public static void UnloadIdsFromCacheByDynamicId(ushort id)
        {
            if (dynamicStaticPairs.TryRemove(id, out long staticId))
            {
                staticDynamicPairs.TryRemove(staticId, out _);
            }

            logger.Debug("Unloaded player ids from cache. dynamic_id={id}", id);
        }
    }
}
