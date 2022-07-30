// <copyright file="ChatMessage.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Chat
{
	public static class ChatMessage
	{
		public static string AnnouncementAdminMutedPlayer(string adminName, string targetName, double durationMinutes, string reason)
		{
			return string.Format("Администратор: {0} выдал мут {1} на {2} минут. Причина: {3}", adminName, targetName, durationMinutes, reason);
		}
	}
}
