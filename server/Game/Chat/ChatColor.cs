// <copyright file="ChatColor.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Chat
{
	using GTANetworkAPI;

	public static class ChatColor
	{
		public const string AdminAnnouncementColor = "!{#ff4633}";
		public const string AdminActionColor = "!{#ffa321}";
		public const string AdminChatColor = "!{#ffdd33}";
		public const string AdminReportColor = "!{#36baf7}";
		public const string AdminReportAnswerColor = "!{#00ab2d}";
		public const string FractionChatColor = "!{#3dcedf}";
		public const string NewPlayerChatColor = "!{#ff9900}";
		public const string VipChatPrefixColor = "!{#6f8cbd}";
		public const string PremiumChatPrefixColor = "!{#a048a8}";

		public static void SendColorizedChatMessageToAll(string color, string message)
		{
			NAPI.Chat.SendChatMessageToAll(color + message);
		}
	}
}
