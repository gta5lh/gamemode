// <copyright file="GetId.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.GameMechanics.Admin.Commands
{
	using System;
	using System.Threading.Tasks;
	using GTANetworkAPI;
	using Gamemode.GameMechanics.Admin.Models;
	using Gamemode.GameMechanics.Player.Models;

	public class SetAdminRank : BaseHandler
	{
		private const string SetAdminRankUsage = "Использование: /setadminrank {static_id} {rank_id}. Пример: /setadminrank 0 1";

		[Command("setadminrank", SetAdminRankUsage, Alias = "sar", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Owner)]
		public async Task OnSetAdminRank(CPlayer admin, string? inputTargetStaticId = null, string? rankId = null)
		{
			if (inputTargetStaticId == null || rankId == null)
			{
				admin.SendChatMessage(SetAdminRankUsage);
				return;
			}

			long targetStaticId;
			AdminRank adminRank;

			try
			{
				targetStaticId = long.Parse(inputTargetStaticId);
				adminRank = rankId.ToAdminRank();
			}
			catch (Exception)
			{
				admin.SendChatMessage("static_id должен быть цифрой. rank_id должен быть от 0 до 5 включительно");
				return;
			}

			// TODO
			// SetAdminRankResponse setAdminRankResponse;

			// try
			// {
			// 	setAdminRankResponse = await Infrastructure.RpcClients.PlayerService.SetAdminRankAsync(new SetAdminRankRequest(targetStaticId, adminRank, admin.StaticId));
			// }
			// catch (Exception)
			// {
			// 	NAPI.Task.Run(() =>
			// 	{
			// 		admin.SendChatMessage($"Пользователь с static_id {targetStaticId} отсутствует");
			// 	});

			// 	return;
			// }

			// AdminRank adminRankBeforeUpdate = setAdminRankResponse.HasOldAdminRankID ? (Models.Admin.AdminRank)setAdminRankResponse.OldAdminRankID : 0;

			// NAPI.Task.Run(() =>
			// {
			// 	CPlayer targetPlayer = PlayerUtil.GetByStaticId(targetStaticId);
			// 	if (targetPlayer != null)
			// 	{
			// 		targetPlayer.AdminRank = adminRank;
			// 	}

			// 	bool isNewRankAdmin = adminRank.IsAdmin();

			// 	if ((adminRankBeforeUpdate >= adminRank && adminRank != 0) || (adminRankBeforeUpdate == 0 && adminRank == 0))
			// 	{
			// 		AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} сменил должность администратора {setAdminRankResponse.Name} на {adminRank.ToPosition()}");
			// 		this.Logger.Warn($"Administrator {admin.Name} set {setAdminRankResponse.Name} as administrator [{adminRank}]");
			// 		return;
			// 	}

			// 	if (isNewRankAdmin)
			// 	{
			// 		Chat.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} назначил {setAdminRankResponse.Name} на должность {adminRank.ToPosition()} администратор. Наши поздравления!");
			// 		this.Logger.Warn($"Administrator {admin.Name} set {setAdminRankResponse.Name} as administrator [{adminRank}]");
			// 	}
			// 	else
			// 	{
			// 		Chat.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} снял {setAdminRankResponse.Name} с должности администратора");
			// 		this.Logger.Warn($"Administrator {admin.Name} removed {setAdminRankResponse.Name} from administrators");
			// 	}
			// });
		}
	}
}
