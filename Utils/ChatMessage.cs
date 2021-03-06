// <copyright file="ChatMessage.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Utils
{
    public static class ChatMessage
    {
        public static string AnnouncementAdminMutedPlayer(string adminName, string targetName, double durationMinutes, string reason)
        {
            return string.Format(Properties.Localization.AnnouncementAdminMutedPlayer, adminName, targetName, durationMinutes, reason);
        }
    }
}
