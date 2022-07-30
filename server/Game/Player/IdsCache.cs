// <copyright file="IdsCache.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Player
{
	using System;
	using System.Collections.Concurrent;

	public static class IdsCache
	{
		private static readonly NLog.ILogger Logger = Gamemode.Logger.Logger.LogFactory.GetCurrentClassLogger();
		private static readonly ConcurrentDictionary<ushort, string> DynamicStaticPairs = new ConcurrentDictionary<ushort, string>();
		private static readonly ConcurrentDictionary<string, ushort> StaticDynamicPairs = new ConcurrentDictionary<string, ushort>();

		public static ushort? DynamicIdByStatic(string id)
		{
			return StaticDynamicPairs.TryGetValue(id, out ushort dynamicId) ? (ushort?)dynamicId : null;
		}

		public static string? StaticIdByDynamic(ushort id)
		{
			return DynamicStaticPairs.TryGetValue(id, out string? staticId) ? (string?)staticId : null;
		}

		public static string? StaticIdByDynamic(string id)
		{
			try
			{
				return DynamicStaticPairs.TryGetValue(ushort.Parse(id), out string? staticId) ? (string?)staticId : null;
			}
			catch (Exception)
			{
				return null;
			}
		}

		public static void LoadIdsToCache(ushort dynamicId, string staticId)
		{
			DynamicStaticPairs[dynamicId] = staticId;
			StaticDynamicPairs[staticId] = dynamicId;
			Logger.Info($"Loaded player ids to cache. dynamic_id={dynamicId}, static_id={staticId}");
		}

		public static void UnloadIdsFromCacheByDynamicId(ushort id)
		{
			if (DynamicStaticPairs.TryRemove(id, out string? staticId))
			{
				StaticDynamicPairs.TryRemove(staticId, out _);
				Logger.Info($"Unloaded player ids from cache. dynamic_id={id}");
			}
		}
	}
}
