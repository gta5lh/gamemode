using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Commands.Player
{
    public class ReportCommand : Script
    {
        private const string ReportCommandUsage = "Использование: /report {сообщение}. Пример: /r ИД 10 читер";

        [Command("report", ReportCommandUsage, Alias = "r", GreedyArg = true)]
        public void AdminChat(CustomPlayer player, string message = null)
        {
            if (message == null)
            {
                player.SendChatMessage(ReportCommandUsage);
                return;
            }

            AdminsCache.SendMessageToAllAdminsReport($"{player.Name} [{player.Id}]: {message}");
        }
    }
}
