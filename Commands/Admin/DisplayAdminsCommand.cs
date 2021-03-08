using Gamemode.Models.Admin;
using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Commands.Admin
{
    class DisplayAdminsCommand : Script
    {
        private const string DisplayAdminsCommandUsage = "Использование: /admins";

        [Command("admins", DisplayAdminsCommandUsage, Alias = "a", SensitiveInfo = true, GreedyArg = true, Hide = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public void DisplayAdmins(CustomPlayer admin)
        {
            admin.SendChatMessage($"Админы онлайн: {AdminsCache.GetAdminNames()}");
        }
    }
}
