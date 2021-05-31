// <copyright file="Chat.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Utils
{
    using GTANetworkAPI;

    public static class Chat
    {
        public static void SendColorizedChatMessageToAll(string color, string message)
        {
            NAPI.Chat.SendChatMessageToAll(color + message);
        }
    }
}
