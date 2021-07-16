using System;
using System.Threading.Tasks;
using Gamemode.Models.Player;
using Gamemode.Utils;
using Grpc.Core;
using GTANetworkAPI;
using Rpc.Errors;
using Rpc.User;

namespace Gamemode.Commands.Admin
{
	public class BanCommand : BaseCommandHandler
	{
		private const string BanCommandUsage = "Использование: /ban {static_id} {дни} {причина}. Пример: /ban 1 31 Спидхак";
		private const string UnbanCommandUsage = "Использование: /unban {static_id}";
		private const int BanInDays = 99999;

		[Command("ban", BanCommandUsage, Alias = "b", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(Models.Admin.AdminRank.Junior)]
		public async Task Ban(CustomPlayer admin, string staticIdInput = null, string durationDays = null, string reason = null)
		{
			if (staticIdInput == null || durationDays == null || reason == null)
			{
				admin.SendChatMessage(BanCommandUsage);
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
				admin.SendChatMessage(BanCommandUsage);
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
				BanResponse banResponse = await Infrastructure.RpcClients.UserService.BanAsync(new BanRequest(staticId, reason, admin.StaticId, bannedAt, bannedUntil));
				targetName = banResponse.Name;
			}
			catch (RpcException ex)
			{
				if (Error.IsEqualErrorCode(ex.StatusCode, ErrorCode.UserNotFound))
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

				CustomPlayer targetPlayer = PlayerUtil.GetByStaticId(staticId);
				if (targetPlayer != null)
				{
					targetPlayer.Ban();
				}
			});
		}

		[Command("unban", UnbanCommandUsage, Alias = "ub", SensitiveInfo = true, Hide = true)]
		[AdminMiddleware(Models.Admin.AdminRank.Junior)]
		public async Task Unban(CustomPlayer admin, string staticIdInput = null)
		{
			if (staticIdInput == null)
			{
				admin.SendChatMessage(UnbanCommandUsage);
				return;
			}

			long staticId;

			try
			{
				staticId = long.Parse(staticIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(UnbanCommandUsage);
				return;
			}

			UnbanResponse unbanResponse;

			try
			{
				unbanResponse = await Infrastructure.RpcClients.UserService.UnbanAsync(new UnbanRequest(staticId, admin.StaticId));
			}
			catch (Exception)
			{
				NAPI.Task.Run(() => admin.SendChatMessage($"Пользователь со static ID {staticId} не найден, либо бан отсутствует"));
				return;
			}

			NAPI.Task.Run(() =>
			{
				Chat.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} снял бан {unbanResponse.Name}");
				this.Logger.Warn($"Administrator {admin.Name} unbanned {unbanResponse.Name}");
			});
		}
	}
}
