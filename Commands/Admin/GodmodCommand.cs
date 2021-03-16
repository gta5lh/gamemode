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
            admin.Invisible = !admin.Invisible;
            NAPI.ClientEvent.TriggerClientEvent(admin, "SetInvisibility");
        }

        //[AdminMiddleware(AdminRank.Junior)]
        //[Command("spectate", InvisibilityCommandUsage, Alias = "s", GreedyArg = true, Hide = true)]
        //public void Spectate(CustomPlayer admin, string targetId)
        //{
        //    var player = PlayerUtil.GetById(ushort.Parse(targetId));

        //    NAPI.ClientEvent.TriggerClientEvent(admin, "Spectate", player.Position.X, player.Position.Y, player.Position.Z, targetId);
        //}

        [RemoteEvent("SetNoclip")]
        private void OnSetNoclip(CustomPlayer admin, string request)
        {
            if (admin.AdminRank == 0)
            {
                return;
            }

            admin.Noclip = bool.Parse(request);
        }
    }
}
