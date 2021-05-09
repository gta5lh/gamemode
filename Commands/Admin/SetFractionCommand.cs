using System;
using System.Threading.Tasks;
using Gamemode.Models.Player;
using Gamemode.Services;
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

            string? targetName;

            try
            {
                CustomPlayer targetPlayer = PlayerUtil.GetByStaticId(staticId);
                targetName = await GangService.SetAsGangMember(targetPlayer, staticId, fractionId, rankId, admin.StaticId);
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
                    AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} сменил фракцию {targetName} на ID={fractionId}, ранг={rankId}");
                    this.Logger.Warn($"Administrator {admin.Name} set fraction of {targetName} to ID={fractionId}. Tier={rankId}");
                }
                else
                {
                    AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} убрал из фракции {targetName}");
                    this.Logger.Warn($"Administrator {admin.Name} unset fraction of {targetName}");
                }
            });
        }
    }
}
