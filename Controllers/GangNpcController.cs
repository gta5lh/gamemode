using System.Threading.Tasks;
using Gamemode.Models.Player;
using Gamemode.Services;
using GTANetworkAPI;
using Newtonsoft.Json;

namespace Gamemode.Controllers
{
    public class GangNpcController : Script
    {
        [RemoteEvent("PlayerSelectedGangNpcAction")]
        private async Task OnPlayerSelectedGangNpcAction(CustomPlayer player, string request)
        {
            PlayerSelectedGangNpcActionRequest playerSelectedGangNpcActionRequest = JsonConvert.DeserializeObject<PlayerSelectedGangNpcActionRequest>(request);

            if (playerSelectedGangNpcActionRequest.Action == "leave")
            {
                this.HandleLeaveAction(player, playerSelectedGangNpcActionRequest.Npc);
            }
            else if (playerSelectedGangNpcActionRequest.Action == "join")
            {
                this.HandleJoinAction(player, playerSelectedGangNpcActionRequest.Npc);
            }
        }

        private async Task HandleLeaveAction(CustomPlayer player, string npc)
        {
            await GangService.SetAsGangMember(player, player.StaticId, 0, 0, player.StaticId);

            NAPI.Task.Run(() =>
            {
                player.SendChatMessage("Давай удачи!");
            });
        }

        private async Task HandleJoinAction(CustomPlayer player, string npc)
        {
            await GangService.SetAsGangMember(player, player.StaticId, GangUtil.GangIdByName[npc], 1, player.StaticId);

            NAPI.Task.Run(() =>
            {
                player.SendChatMessage("Добро пожаловать в наши ряды!");
            });
        }

        public class PlayerSelectedGangNpcActionRequest
        {
            public string Action { get; set; }

            public string Npc { get; set; }
        }
    }
}
