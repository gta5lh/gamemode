// <copyright file="MuteCommand.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Commands.Admin
{
	using System;
	using System.Threading.Tasks;
	using Gamemode.Models.Player;
	using Gamemode.Utils;
	using GTANetworkAPI;

	public class MuteCommand : BaseCommandHandler
	{
		private const string MuteCommandUsage = "Использование: /mute {static_id} {минуты} {причина}. Пример: /m 1 100 Оскорбления";
		private const string UnmuteCommandUsage = "Использование: /unmute {static_id}. Пример: /um 1";
		private const int MonthInMinutes = 44640;

		[AdminMiddleware(Models.Admin.AdminRank.Junior)]
		[Command("mute", MuteCommandUsage, Alias = "m", GreedyArg = true, Hide = true)]
		public async Task Mute(CustomPlayer admin, string playerId = null, string durationMinutes = null, string reason = null)
		{
			if (playerId == null || durationMinutes == null || reason == null)
			{
				admin.SendChatMessage(MuteCommandUsage);
				return;
			}

			long targetId;
			int duration;

			try
			{
				targetId = long.Parse(playerId);
				duration = int.Parse(durationMinutes);
			}
			catch (Exception)
			{
				admin.SendChatMessage(MuteCommandUsage);
				return;
			}

			if (duration <= 0 || duration > MonthInMinutes)
			{
				admin.SendChatMessage($"Мут можно выдать минимум на 1 минуту, максимум на {MonthInMinutes}");
				return;
			}

			DateTime mutedUntil = DateTime.UtcNow.AddMinutes(duration);
			DateTime mutedAt = DateTime.UtcNow;

			string targetName;

			try
			{
				targetName = await ApiClient.ApiClient.MuteUser(targetId, reason, admin.StaticId, mutedAt, mutedUntil);
			}
			catch (Exception)
			{
				NAPI.Task.Run(() => admin.SendChatMessage($"Пользователь со static ID {targetId} не найден"));
				return;
			}

			NAPI.Task.Run(() =>
			{
				CustomPlayer targetPlayer = PlayerUtil.GetByStaticId(targetId);
				if (targetPlayer != null)
				{
					targetPlayer.MuteState = new MuteState(mutedUntil, admin.StaticId, reason);
				}

				Chat.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} выдал мут {targetName} на {duration} минут. Причина: {reason}");
				this.Logger.Warn($"Administrator {admin.Name} muted {targetName} for {duration} minutes");
			});
		}

		[Command("unmute", UnmuteCommandUsage, Alias = "um", GreedyArg = true, Hide = true)]
		[AdminMiddleware(Models.Admin.AdminRank.Junior)]
		public async Task Unmute(CustomPlayer admin, string playerId = null)
		{
			if (playerId == null)
			{
				admin.SendChatMessage(UnmuteCommandUsage);
				return;
			}

			long targetId;

			try
			{
				targetId = long.Parse(playerId);
			}
			catch (Exception)
			{
				admin.SendChatMessage(UnmuteCommandUsage);
				return;
			}

			string targetName;

			try
			{
				targetName = await ApiClient.ApiClient.UnmuteUser(targetId, admin.StaticId);
			}
			catch (Exception)
			{
				NAPI.Task.Run(() => admin.SendChatMessage($"Пользователь со static ID {targetId} не найден, либо мут отсутствует"));
				return;
			}

			NAPI.Task.Run(() =>
			{
				CustomPlayer targetPlayer = PlayerUtil.GetByStaticId(targetId);
				if (targetPlayer != null)
				{
					targetPlayer.MuteState = new MuteState();
				}

				Chat.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} снял мут {targetName}");
				this.Logger.Warn($"Administrator {admin.Name} unmuted {targetName}");
			});
		}
	}
}
