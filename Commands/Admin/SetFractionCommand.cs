using System;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
using Gamemode.Models.Gangs;
using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Commands.Admin
{
    public class SetFractionCommand : BaseCommandHandler
    {
        private const string MakeMemberCommandUsage = "Использование: /setfraction {static_id} {fraction_id} {rank_id}. Пример: /setfraction 10 1 9";

        [Command("setfraction", MakeMemberCommandUsage, Alias = "sf", GreedyArg = true, Hide = true)]
        [AdminMiddleware(Models.Admin.AdminRank.Junior)]
        public async Task MakeMember(CustomPlayer admin, string staticIdInput = null, string fractionIdInput = null, string rankIdInput = null)
        {
            if (staticIdInput == null || staticIdInput == null || fractionIdInput == null || rankIdInput == null)
            {
                admin.SendChatMessage(MakeMemberCommandUsage);
                return;
            }

            long staticId;
            byte fractionId;
            byte rankId;

            try
            {
                staticId = long.Parse(staticIdInput);
                fractionId = byte.Parse(fractionIdInput);
                rankId = byte.Parse(rankIdInput);
            }
            catch (Exception)
            {
                admin.SendChatMessage(MakeMemberCommandUsage);
                return;
            }

            SetFractionResponse setFractionResponse;

            try
            {
                setFractionResponse = await ApiClient.ApiClient.SetFraction(staticId, fractionId, rankId, admin.StaticId);
            }
            catch (Exception)
            {
                NAPI.Task.Run(() => admin.SendChatMessage($"Пользователь со static ID {staticId} не найден"));
                return;
            }

            NAPI.Task.Run(() =>
            {
                if (fractionId != 0)
                {
                    AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} сменил фракцию {setFractionResponse.Name} на ID={fractionId}, ранг={rankId}");
                    this.Logger.Warn($"Administrator {admin.Name} set fraction of {setFractionResponse.Name} to ID={fractionId}. Tier={rankId}");
                }
                else
                {
                    AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} убрал из фракции {setFractionResponse.Name}");
                    this.Logger.Warn($"Administrator {admin.Name} unset fraction of {setFractionResponse.Name}");
                }

                CustomPlayer targetPlayer = PlayerUtil.GetByStaticId(staticId);
                if (targetPlayer != null)
                {
                    targetPlayer.Fraction = fractionId != 0 ? (byte?)fractionId : null;
                    targetPlayer.FractionRank = fractionId != 0 ? (byte?)rankId : null;
                    targetPlayer.FractionRankName = setFractionResponse.TierName != null ? setFractionResponse.TierName : null;
                    targetPlayer.RequiredExperience = setFractionResponse.TierRequiredExperience != null ? (short?)setFractionResponse.TierRequiredExperience : null;
                    targetPlayer.CurrentExperience = 0;
                    targetPlayer.SetClothes(Clothes.GangClothes[fractionId]);
                }
            });
        }
    }
}
