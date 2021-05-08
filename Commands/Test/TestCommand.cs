using System;
using Gamemode.Commands.Admin;
using Gamemode.Models.Admin;
using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Commands.Test
{
    public class TestCommand : Script
    {
        [Command("tmoney", "/tmoney", Alias = "tm", GreedyArg = true)]
        [AdminMiddleware(AdminRank.Owner)]
        public void Money(CustomPlayer player, string message = null)
        {
            player.SendChatMessage($"{player.Money}");
        }

        [Command("tlogin", "/tlogin", Alias = "tl", GreedyArg = true)]
        [AdminMiddleware(AdminRank.Owner)]
        public void Login(CustomPlayer player, string message = null)
        {
            player.LoggedInAt = DateTime.UtcNow;
            player.SendChatMessage($"{player.LoggedInAt}");
        }
    }
}
