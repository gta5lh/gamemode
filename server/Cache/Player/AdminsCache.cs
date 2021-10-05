using System.Collections.Generic;
using Gamemode.Utils;
using GTANetworkAPI;

namespace Gamemode.Cache.Player
{
	public class AdminsCache
	{
		private static readonly Dictionary<long, string> Admins = new Dictionary<long, string>();

		public static void LoadAdminToCache(long staticId, string name)
		{
			if (Admins.ContainsKey(staticId))
			{
				return;
			}

			Admins.Add(staticId, name);
		}

		public static void UnloadAdminFromCache(long staticId)
		{
			if (!Admins.ContainsKey(staticId))
			{
				return;
			}

			Admins.Remove(staticId);
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
