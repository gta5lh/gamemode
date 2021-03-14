using Gamemode.Models.Admin;
using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Commands.Admin
{
    public class GodmodCommand : BaseCommandHandler
    {
        private const string GodmodCommandUsage = "Использование: /godmod. Пример: /gm";

        [Command("godmod", GodmodCommandUsage, Alias = "gm", GreedyArg = true, Hide = true)]
        [AdminMiddleware(AdminRank.Junior)]
        public void Godmod(CustomPlayer admin)
        {
            NAPI.ClientEvent.TriggerClientEvent(admin, "SetGodmod");
        }

        private const string InvisibilityCommandUsage = "Использование: /invisibility. Пример: /i";

        [AdminMiddleware(AdminRank.Junior)]
        [Command("invisibility", InvisibilityCommandUsage, Alias = "i", GreedyArg = true, Hide = true)]
        public void Invisibility(CustomPlayer admin)
        {
            NAPI.ClientEvent.TriggerClientEvent(admin, "SetInvisibility");
        }
    }
}
