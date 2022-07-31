// <copyright file="Cache.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Admin
{
	using System.Collections.Generic;
	using Gamemode.Game.Chat;
	using GTANetworkAPI;

	public static class Cache
	{
		private static readonly NLog.ILogger Logger = Gamemode.Logger.Logger.LogFactory.GetCurrentClassLogger();
		private static readonly Dictionary<string, string> Admins = new();

		public static void LoadAdminToCache(string staticId, string name)
		{
			if (Admins.ContainsKey(staticId))
			{
				return;
			}

			Admins.Add(staticId, name);
			Logger.Info($"Loaded admin to cache. name={name}, static_id={staticId}");
		}

		public static void UnloadAdminFromCache(string staticId)
		{
			if (!Admins.ContainsKey(staticId))
			{
				return;
			}

			Admins.Remove(staticId);
			Logger.Info($"Unloaded admin from cache. static_id={staticId}");
		}

		public static string GetAdminNames()
		{
			return string.Join(", ", Admins.Values);
		}

		public static void SendMessageToAllAdminsAction(string message)
		{
			foreach (string adminStaticId in Admins.Keys)
			{
				NAPI.Chat.SendChatMessageToPlayer(PlayerUtil.GetByStaticId(adminStaticId), $"{ChatColor.AdminActionColor}[AF] {message}");
			}
		}

		public static void SendMessageToAllAdminsChat(string message)
		{
			foreach (string adminStaticId in Admins.Keys)
			{
				NAPI.Chat.SendChatMessageToPlayer(PlayerUtil.GetByStaticId(adminStaticId), $"{ChatColor.AdminChatColor}[AC] {message}");
			}
		}

		public static void SendMessageToAllAdminsReport(string message)
		{
			foreach (string adminStaticId in Admins.Keys)
			{
				NAPI.Chat.SendChatMessageToPlayer(PlayerUtil.GetByStaticId(adminStaticId), $"{ChatColor.AdminReportColor}[AR] {message}");
			}
		}

		public static void SendMessageToAllAdminsReportAnswer(string message)
		{
			foreach (string adminStaticId in Admins.Keys)
			{
				NAPI.Chat.SendChatMessageToPlayer(PlayerUtil.GetByStaticId(adminStaticId), $"{ChatColor.AdminReportAnswerColor}[AR] {message}");
			}
		}
	}
}
