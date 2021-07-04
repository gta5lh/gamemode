using System.Collections.Concurrent;
using Gamemode.Utils;
using GTANetworkAPI;

namespace Gamemode.Models.Player
{
	public class AdminsCache
	{
		private static readonly NLog.ILogger Logger = Gamemode.Logger.Logger.LogFactory.GetCurrentClassLogger();
		private static readonly ConcurrentDictionary<long, string> Admins = new ConcurrentDictionary<long, string>();

		public static void LoadAdminToCache(long staticId, string name)
		{
			if (Admins.ContainsKey(staticId))
			{
				return;
			}

			Admins[staticId] = name;
			Logger.Info($"Loaded admin to cache. static_id={staticId}");
		}

		public static void UnloadAdminFromCache(long staticId)
		{
			if (!Admins.ContainsKey(staticId))
			{
				return;
			}

			Admins.TryRemove(staticId, out _);
			Logger.Info($"Unloaded admin from cache. static_id={staticId}");
		}

		public static string GetAdminNames()
		{
			return string.Join(", ", Admins.Values);
		}

		public static void SendMessageToAllAdminsAction(string message)
		{
			foreach (long adminStaticId in Admins.Keys)
			{
				NAPI.Chat.SendChatMessageToPlayer(PlayerUtil.GetByStaticId(adminStaticId), $"{ChatColor.AdminActionColor}[AF] {message}");
			}
		}

		public static void SendMessageToAllAdminsChat(string message)
		{
			foreach (long adminStaticId in Admins.Keys)
			{
				NAPI.Chat.SendChatMessageToPlayer(PlayerUtil.GetByStaticId(adminStaticId), $"{ChatColor.AdminChatColor}[AC] {message}");
			}
		}

		public static void SendMessageToAllAdminsReport(string message)
		{
			foreach (long adminStaticId in Admins.Keys)
			{
				NAPI.Chat.SendChatMessageToPlayer(PlayerUtil.GetByStaticId(adminStaticId), $"{ChatColor.AdminReportColor}[AR] {message}");
			}
		}

		public static void SendMessageToAllAdminsReportAnswer(string message)
		{
			foreach (long adminStaticId in Admins.Keys)
			{
				NAPI.Chat.SendChatMessageToPlayer(PlayerUtil.GetByStaticId(adminStaticId), $"{ChatColor.AdminReportAnswerColor}[AR] {message}");
			}
		}
	}
}
