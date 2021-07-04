using System;
using System.Threading.Tasks;
using Gamemode.Models.Player;
using Gamemode.Utils;
using GTANetworkAPI;

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


			DateTime bannedUntil = DateTime.UtcNow.AddDays(duration);
			DateTime bannedAt = DateTime.UtcNow;

			string targetName;

			try
			{
				targetName = await ApiClient.ApiClient.BanUser(staticId, reason, admin.StaticId, bannedAt, bannedUntil);
			}
			catch (Exception)
			{
				NAPI.Task.Run(() => admin.SendChatMessage($"Пользователь со static ID {staticId} не найден"));
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

			string targetName;

			try
			{
				targetName = await ApiClient.ApiClient.UnbanUser(staticId, admin.StaticId);
			}
			catch (Exception)
			{
				NAPI.Task.Run(() => admin.SendChatMessage($"Пользователь со static ID {staticId} не найден, либо бан отсутствует"));
				return;
			}

			NAPI.Task.Run(() =>
			{
				Chat.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} снял бан {targetName}");
				this.Logger.Warn($"Administrator {admin.Name} unbanned {targetName}");
			});
		}
	}
}
