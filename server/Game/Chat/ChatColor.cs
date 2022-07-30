// <copyright file="ChatColor.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using GTANetworkAPI;

namespace Gamemode.Game.Chat
{
	public static class ChatColor
	{
		public static void SendColorizedChatMessageToAll(string color, string message)
		{
			NAPI.Chat.SendChatMessageToAll(color + message);
		}

		public const string AdminAnnouncementColor = "!{#ff4633}";
		public const string AdminActionColor = "!{#ffa321}";
		public const string AdminChatColor = "!{#ffdd33}";
		public const string AdminReportColor = "!{#36baf7}";
		public const string AdminReportAnswerColor = "!{#00ab2d}";
		public const string FractionChatColor = "!{#3dcedf}";
		public const string NewPlayerChatColor = "!{#ff9900}";
		public const string VipChatPrefixColor = "!{#6f8cbd}";
		public const string PremiumChatPrefixColor = "!{#a048a8}";
	}
}
