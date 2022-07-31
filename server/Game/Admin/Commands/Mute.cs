// <copyright file="Mute.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Admin.Commands
{
	using System;
	using System.Threading.Tasks;
	using Gamemode.Game.Admin.Models;
	using Gamemode.Game.Chat;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;
	using Rpc.Player;

	public class Mute : BaseHandler
	{
		private const string MuteUsage = "Использование: /mute {static_id} {минуты} {причина}. Пример: /m 1 100 Оскорбления";
		private const string UnMuteUsage = "Использование: /unmute {static_id}. Пример: /um 1";
		private const int MonthInMinutes = 44640;

		[AdminMiddleware(AdminRank.Junior)]
		[Command("mute", MuteUsage, Alias = "m", GreedyArg = true, Hide = true)]
		public async Task OnMute(CPlayer admin, string? playerId = null, string? durationMinutes = null, string? reason = null)
		{
			if (playerId == null || durationMinutes == null || reason == null)
			{
				admin.SendChatMessage(MuteUsage);
				return;
			}

			string targetId;
			int duration;

			try
			{
				targetId = playerId;
				duration = int.Parse(durationMinutes);
			}
			catch (Exception)
			{
				admin.SendChatMessage(MuteUsage);
				return;
			}

			if (duration <= 0 || duration > MonthInMinutes)
			{
				admin.SendChatMessage($"Мут можно выдать минимум на 1 минуту, максимум на {MonthInMinutes}");
				return;
			}

			DateTime mutedAt = DateTime.UtcNow;
			DateTime mutedUntil = mutedAt.AddMinutes(duration);

			MuteResponse muteResponse;

			try
			{
				muteResponse = await Infrastructure.RpcClients.PlayerService.MuteAsync(new MuteRequest(targetId, reason, admin.PKId, mutedAt, mutedUntil));
			}
			catch (Exception)
			{
				NAPI.Task.Run(() => admin.SendChatMessage($"Пользователь со static ID {targetId} не найден"));
				return;
			}

			NAPI.Task.Run(() =>
			{
				CPlayer? targetPlayer = PlayerUtil.GetByStaticId(targetId);
				if (targetPlayer != null)
				{
					targetPlayer.MuteState = new MuteState(mutedUntil, admin.StaticId, reason);

					// VoiceChatController.Mute(targetPlayer);
				}

				ChatColor.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} выдал мут {muteResponse.Name} на {duration} минут. Причина: {reason}");
				this.Logger.Warn($"Administrator {admin.Name} muted {muteResponse.Name} for {duration} minutes");
			});
		}

		[Command("unmute", UnMuteUsage, Alias = "um", GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public async Task OnUnmute(CPlayer admin, string? playerId = null)
		{
			if (playerId == null)
			{
				admin.SendChatMessage(UnMuteUsage);
				return;
			}

			string targetId;

			try
			{
				targetId = playerId;
			}
			catch (Exception)
			{
				admin.SendChatMessage(UnMuteUsage);
				return;
			}

			UnmuteResponse unmuteResponse;

			try
			{
				unmuteResponse = await Infrastructure.RpcClients.PlayerService.UnmuteAsync(new UnmuteRequest(targetId, admin.PKId));
			}
			catch (Exception)
			{
				NAPI.Task.Run(() => admin.SendChatMessage($"Пользователь со static ID {targetId} не найден, либо мут отсутствует"));
				return;
			}

			NAPI.Task.Run(() =>
			{
				CPlayer? targetPlayer = PlayerUtil.GetByStaticId(targetId);
				if (targetPlayer != null)
				{
					targetPlayer.MuteState = new MuteState();

					// VoiceChatController.Unmute(targetPlayer);
				}

				ChatColor.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} снял мут {unmuteResponse.Name}");
				this.Logger.Warn($"Administrator {admin.Name} unmuted {unmuteResponse.Name}");
			});
		}
	}
}
