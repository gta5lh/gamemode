// <copyright file="Cache.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Fraction
{
	using System.Collections.Generic;
	using Gamemode.Game.Chat;
	using GTANetworkAPI;

	public static class Cache
	{
		private static readonly Dictionary<byte, Dictionary<string, string>> FractionMembers = new Dictionary<byte, Dictionary<string, string>>()
		{
			{ 1, new Dictionary<string, string>() },
			{ 2, new Dictionary<string, string>() },
			{ 3, new Dictionary<string, string>() },
			{ 4, new Dictionary<string, string>() },
			{ 5, new Dictionary<string, string>() },
		};

		public static void LoadFractionMemberToCache(byte fractionId, string staticId, string name)
		{
			if (FractionMembers[fractionId].ContainsKey(staticId))
			{
				return;
			}

			FractionMembers[fractionId][staticId] = name;
		}

		public static void UnloadFractionMemberFromCache(byte fractionId, string staticId)
		{
			if (!FractionMembers[fractionId].ContainsKey(staticId))
			{
				return;
			}

			FractionMembers[fractionId].Remove(staticId);
		}

		public static void SendMessageToAllFractionMembers(byte fractionId, string message)
		{
			foreach (string staticId in FractionMembers[fractionId].Keys)
			{
				NAPI.Chat.SendChatMessageToPlayer(Gamemode.Game.Player.Util.GetByStaticId(staticId), $"{ChatColor.FractionChatColor}[F] {message}");
			}
		}
	}
}
