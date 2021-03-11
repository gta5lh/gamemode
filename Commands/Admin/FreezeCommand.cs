using System;
using Gamemode.Models.Admin;
using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Commands.Admin
{
    public class FreezeCommand : BaseCommandHandler
    {
        private const string FreezeCommandUsage = "Использование: /freeze {player_id}. Пример: /freeze 10";

        [Command("freeze", FreezeCommandUsage, Alias = "fe", SensitiveInfo = true, GreedyArg = true, Hide = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public void Freeze(CustomPlayer admin, string playerIdInput = null)
        {
            if (playerIdInput == null)
            {
                admin.SendChatMessage(FreezeCommandUsage);
                return;
            }

            ushort playerId = 0;

            try
            {
                playerId = ushort.Parse(playerIdInput);
            }
            catch (Exception)
            {
                admin.SendChatMessage(FreezeCommandUsage);
                return;
            }

            CustomPlayer targetPlayer = PlayerUtil.GetById(playerId);
            if (targetPlayer == null)
            {
                admin.SendChatMessage($"Пользователь с DID {playerId} не найден");
                return;
            }

            targetPlayer.Freezed = !targetPlayer.Freezed;
            if (targetPlayer.Freezed && targetPlayer.IsInVehicle)
            {
                targetPlayer.WarpOutOfVehicle();
            }

            NAPI.ClientEvent.TriggerClientEvent(targetPlayer, "FreezePlayer", targetPlayer.Freezed);

            string freezeString = targetPlayer.Freezed ? "заморозил" : "разморозил";
            AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} [{admin.StaticId}] {freezeString} {targetPlayer.Name} [{targetPlayer.StaticId}]");
            this.Logger.Warn($"Administrator {admin.Name} freezed({targetPlayer.Freezed}) {targetPlayer.Name}");
        }
    }
}
