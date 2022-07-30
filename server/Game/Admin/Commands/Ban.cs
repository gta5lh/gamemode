// <copyright file="Ban.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Admin.Commands
{
	using System;
	using System.Threading.Tasks;
	using Gamemode.Game.Admin.Models;
	using Gamemode.Game.Chat;
	using Gamemode.Game.Player.Models;
	using Grpc.Core;
	using GTANetworkAPI;
	using Rpc.Errors;
	using Rpc.Player;

	public class Ban : BaseHandler
	{
		private const string BanUsage = "Использование: /ban {static_id} {дни} {причина}. Пример: /ban 1 31 Спидхак";
		private const string UnBanUsage = "Использование: /unban {static_id}";
		private const int MaxBanInDays = 99999;

		[Command("ban", BanUsage, Alias = "b", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public async Task OnBan(CPlayer admin, string? staticIdInput = null, string? durationDays = null, string? reason = null)
		{
			if (staticIdInput == null || durationDays == null || reason == null)
			{
				admin.SendChatMessage(BanUsage);
				return;
			}

			string staticId;
			int duration;

			try
			{
				staticId = staticIdInput;
				duration = int.Parse(durationDays);
			}
			catch (Exception)
			{
				admin.SendChatMessage(BanUsage);
				return;
			}

			if (duration <= 0 || duration > MaxBanInDays)
			{
				admin.SendChatMessage($"Бан можно выдать минимум на 1 день, максимум на {MaxBanInDays}");
				return;
			}

			DateTime bannedAt = DateTime.UtcNow;
			DateTime bannedUntil = bannedAt.AddDays(duration);

			string targetName = string.Empty;

			try
			{
				BanResponse banResponse = await Infrastructure.RpcClients.PlayerService.BanAsync(new BanRequest(staticId, reason, admin.PKId, bannedAt, bannedUntil));
				targetName = banResponse.Name;
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
				NAPI.Task.Run(() => admin.SendChatMessage("Что-то пошло не так, попробуй снова"));
				return;
			}

			NAPI.Task.Run(() =>
			{
				ChatColor.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} выдал бан {targetName} на {duration} дней. Причина: {reason}");
				this.Logger.Warn($"Administrator {admin.Name} banned {targetName} for {duration} days");

				CPlayer? targetPlayer = PlayerUtil.GetByStaticId(staticId);
				targetPlayer?.Ban();
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

			string staticId;

			try
			{
				staticId = staticIdInput;
			}
			catch (Exception)
			{
				admin.SendChatMessage(UnBanUsage);
				return;
			}

			UnbanResponse unbanResponse;

			try
			{
				unbanResponse = await Infrastructure.RpcClients.PlayerService.UnbanAsync(new UnbanRequest(staticId, admin.PKId));
			}
			catch (Exception)
			{
				NAPI.Task.Run(() => admin.SendChatMessage($"Пользователь со static ID {staticId} не найден, либо бан отсутствует"));
				return;
			}

			NAPI.Task.Run(() =>
			{
				ChatColor.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} снял бан {unbanResponse.Name}");
				this.Logger.Warn($"Administrator {admin.Name} unbanned {unbanResponse.Name}");
			});
		}
	}
}
