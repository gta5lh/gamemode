using System;
using System.Threading.Tasks;
using Gamemode.GameMechanics.Admin.Models;
using Gamemode.GameMechanics.Player.Models;
using Gamemode.Utils;
using Grpc.Core;
using GTANetworkAPI;
using Rpc.Errors;
using Rpc.Player;

namespace Gamemode.GameMechanics.Admin.Commands
{
	public class Ban : BaseHandler
	{
		private const string BanUsage = "Использование: /ban {static_id} {дни} {причина}. Пример: /ban 1 31 Спидхак";
		private const string UnBanUsage = "Использование: /unban {static_id}";
		private const int BanInDays = 99999;

		[Command("ban", BanUsage, Alias = "b", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public async Task OnBan(CPlayer admin, string? staticIdInput = null, string? durationDays = null, string? reason = null)
		{
			if (staticIdInput == null || durationDays == null || reason == null)
			{
				admin.SendChatMessage(BanUsage);
				return;
			}

			long staticId;
			int duration;

			try
			{
				staticId = long.Parse(staticIdInput);
				duration = int.Parse(durationDays);
			}
			catch (Exception)
			{
				admin.SendChatMessage(BanUsage);
				return;
			}

			if (duration <= 0 || duration > BanInDays)
			{
				admin.SendChatMessage($"Бан можно выдать минимум на 1 день, максимум на {BanInDays}");
				return;
			}


			DateTime bannedAt = DateTime.UtcNow;
			DateTime bannedUntil = bannedAt.AddDays(duration);

			string targetName = "";

			try
			{
				// TODO
				// BanResponse banResponse = await Infrastructure.RpcClients.PlayerService.BanAsync(new BanRequest(staticId, reason, admin.StaticId, bannedAt, bannedUntil));
				// targetName = banResponse.Name;
			}
			catch (RpcException ex)
			{
				if (Error.IsEqualErrorCode(ex.StatusCode, ErrorCode.PlayerNotFound))
				{
					NAPI.Task.Run(() => admin.SendChatMessage($"Пользователь со static ID {staticId} не найден"));
				}

				return;
			}
			catch (Exception)
			{
				NAPI.Task.Run(() => admin.SendChatMessage($"Что-то пошло не так, попробуй снова"));
				return;
			}

			NAPI.Task.Run(() =>
			{
				Chat.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} выдал бан {targetName} на {duration} дней. Причина: {reason}");
				this.Logger.Warn($"Administrator {admin.Name} banned {targetName} for {duration} days");

				// TODO
				// CPlayer targetPlayer = PlayerUtil.GetByStaticId(staticId);
				// if (targetPlayer != null)
				// {
				// 	targetPlayer.Ban();
				// }
			});
		}

		[Command("unban", UnBanUsage, Alias = "ub", SensitiveInfo = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public async Task Unban(CPlayer admin, string? staticIdInput = null)
		{
			if (staticIdInput == null)
			{
				admin.SendChatMessage(UnBanUsage);
				return;
			}

			long staticId;

			try
			{
				staticId = long.Parse(staticIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(UnBanUsage);
				return;
			}

			// TODO
			// UnbanResponse unbanResponse;

			// try
			// {
			// TODO
			// unbanResponse = await Infrastructure.RpcClients.PlayerService.UnbanAsync(new UnbanRequest(staticId, admin.StaticId));
			// }
			// catch (Exception)
			// {
			// 	NAPI.Task.Run(() => admin.SendChatMessage($"Пользователь со static ID {staticId} не найден, либо бан отсутствует"));
			// 	return;
			// }

			// NAPI.Task.Run(() =>
			// {
			// 	Chat.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} снял бан {unbanResponse.Name}");
			// 	this.Logger.Warn($"Administrator {admin.Name} unbanned {unbanResponse.Name}");
			// });
		}
	}
}
