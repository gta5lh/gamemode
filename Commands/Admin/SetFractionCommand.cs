using System;
using System.Threading.Tasks;
using Gamemode.Models.Gangs;
using Gamemode.Models.Player;
using Gamemode.Repositories.Models;
using Gamemode.Repository;
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

            User user = await UserRepository.SetFraction(staticId, fractionId, rankId);
            if (user == null)
            {
                NAPI.Task.Run(() => admin.SendChatMessage($"Пользователь со static ID {staticId} не найден"));
                return;
            }

            NAPI.Task.Run(() =>
            {
                if (fractionId != 0)
                {
                    AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} сменил фракцию {user.Name} на ID={user.FractionId}, ранг={user.FractionRank.Tier}");
                    this.Logger.Warn($"Administrator {admin.Name} set fraction of {user.Name} to Fraction={user.FractionId}. Tier={user.FractionRank.Tier}");
                }
                else
                {
                    AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} убрал из фракции {user.Name}");
                    this.Logger.Warn($"Administrator {admin.Name} unset fraction of {user.Name}");
                }

                CustomPlayer targetPlayer = PlayerUtil.GetByStaticId(staticId);
                if (targetPlayer != null)
                {
                    targetPlayer.Fraction = user.FractionId;
                    targetPlayer.FractionRank = user.FractionRank != null ? (byte?)user.FractionRank.Tier : null;
                    targetPlayer.FractionRankName = user.FractionRank != null ? user.FractionRank.Name : null;
                    targetPlayer.RequiredExperience = user.FractionRank != null ? (short?)user.FractionRank.RequiredExperienceToRankUp : null;
                    targetPlayer.CurrentExperience = 0;
                    targetPlayer.SetClothes(Clothes.GangClothes[user.FractionId != null ? (byte)user.FractionId : (byte)0]);
                }
            });
        }
    }
}
