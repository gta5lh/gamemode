using System;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
using Gamemode.Models.Gangs;
using Gamemode.Models.Player;
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
            SetFractionResponse setFractionResponse;
            byte fractionId = GangUtil.GangIdByName[npc];

            try
            {
                setFractionResponse = await ApiClient.ApiClient.SetFraction(player.StaticId, 0, 0, player.StaticId);
            }
            catch (Exception)
            {
                NAPI.Task.Run(() => player.SendChatMessage($"Что-то пошло не так, попробуй вступить еще раз"));
                return;
            }

            NAPI.Task.Run(() =>
            {
                player.Fraction = null;
                player.FractionRank = null;
                player.FractionRankName = null;
                player.RequiredExperience = null;
                player.CurrentExperience = 0;
                player.SetClothes(Clothes.GangClothes[0]);
                player.SendChatMessage("Давай удачи!");
            });
        }

        private async Task HandleJoinAction(CustomPlayer player, string npc)
        {
            SetFractionResponse setFractionResponse;
            byte fractionId = GangUtil.GangIdByName[npc];

            try
            {
                setFractionResponse = await ApiClient.ApiClient.SetFraction(player.StaticId, fractionId, 1, player.StaticId);
            }
            catch (Exception)
            {
                NAPI.Task.Run(() => player.SendChatMessage($"Что-то пошло не так, попробуй вступить еще раз"));
                return;
            }

            NAPI.Task.Run(() =>
            {
                player.Fraction = fractionId;
                player.FractionRank = 1;
                player.FractionRankName = setFractionResponse.TierName;
                player.RequiredExperience = (short?)setFractionResponse.TierRequiredExperience;
                player.CurrentExperience = 0;
                player.SetClothes(Clothes.GangClothes[fractionId]);
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
