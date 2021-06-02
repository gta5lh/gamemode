using System;
using Gamemode.Commands.Admin;
using Gamemode.Controllers;
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

        [Command("testa")]
        [AdminMiddleware(AdminRank.Owner)]
        public void TestA(CustomPlayer sender)
        {
            GangZoneController.OnCaptureStart(2);
            sender.SendChatMessage("Started");
        }

        [Command("testb")]
        [AdminMiddleware(AdminRank.Owner)]
        public void TestB(CustomPlayer sender)
        {
            GangZoneController.OnCaptureEnd(2, 2);
            sender.SendChatMessage("Ended");
        }

        [Command("capt")]
        [AdminMiddleware(AdminRank.Owner)]
        public void Capt(CustomPlayer sender)
        {
            int blip = GangZoneController.TryCaptureStart(sender); // Возвращает id захватываемой территории
            if (blip == -1) return;
            sender.SendChatMessage("Found");
        }

        [Command("rb")]
        [AdminMiddleware(AdminRank.Owner)]
        public async void Rb(CustomPlayer player)
        {
            var zones = await GangZoneController.LoadZones();
            NAPI.ClientEventThreadSafe.TriggerClientEvent(player, "RenderGangZones", zones);
        }
    }
}
