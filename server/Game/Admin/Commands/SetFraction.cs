// <copyright file="SetFraction.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Admin.Commands
{
	using System;
	using System.Threading.Tasks;
	using Gamemode.Game.Admin.Models;
	using Gamemode.Game.Gang;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class SetFraction : BaseHandler
	{
		private const string SetFractionUsage = "Использование: /setfraction {static_id} {fraction_id} {rank_id}. Пример: /setfraction 10 1 9";

		[Command("setfraction", SetFractionUsage, Alias = "sf", GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public async Task OnSetFraction(CPlayer admin, string? staticIdInput = null, string? fractionIdInput = null, string? rankIdInput = null)
		{
			if (staticIdInput == null || fractionIdInput == null || rankIdInput == null)
			{
				admin.SendChatMessage(SetFractionUsage);
				return;
			}

			string staticId;
			byte fractionId;
			byte rankId;

			try
			{
				staticId = staticIdInput;
				fractionId = byte.Parse(fractionIdInput);
				rankId = byte.Parse(rankIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(SetFractionUsage);
				return;
			}

			string targetName;

			try
			{
				CPlayer? targetPlayer = PlayerUtil.GetByStaticId(staticId);

				targetName = await GangMgr.SetAsGangMember(targetPlayer!, staticId, fractionId, rankId, admin.PKId);
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
					Cache.SendMessageToAllAdminsAction($"{admin.Name} сменил фракцию {targetName} на ID={fractionId}, ранг={rankId}");
					this.Logger.Warn($"Administrator {admin.Name} set fraction of {targetName} to ID={fractionId}. Tier={rankId}");
				}
				else
				{
					Cache.SendMessageToAllAdminsAction($"{admin.Name} убрал из фракции {targetName}");
					this.Logger.Warn($"Administrator {admin.Name} unset fraction of {targetName}");
				}
			});
		}
	}
}
