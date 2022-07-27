﻿// <copyright file="MuteCommand.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.GameMechanics.Admin.Commands
{
	using System;
	using System.Threading.Tasks;
	using Gamemode.GameMechanics.Admin.Models;
	using Gamemode.GameMechanics.Player.Models;
	using GTANetworkAPI;

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

			long targetId;
			int duration;

			try
			{
				targetId = long.Parse(playerId);
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

			// TODO
			// MuteResponse muteResponse;

			// try
			// {
			// 	muteResponse = await Infrastructure.RpcClients.PlayerService.MuteAsync(new MuteRequest(targetId, reason, admin.StaticId, mutedAt, mutedUntil));
			// }
			// catch (Exception)
			// {
			// 	NAPI.Task.Run(() => admin.SendChatMessage($"Пользователь со static ID {targetId} не найден"));
			// 	return;
			// }

			// NAPI.Task.Run(() =>
			// {
			// 	CPlayer targetPlayer = PlayerUtil.GetByStaticId(targetId);
			// 	if (targetPlayer != null)
			// 	{
			// 		targetPlayer.MuteState = new MuteState(mutedUntil, admin.StaticId, reason);
			// 		VoiceChatController.Mute(targetPlayer);
			// 	}

			// 	Chat.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} выдал мут {muteResponse.Name} на {duration} минут. Причина: {reason}");
			// 	this.Logger.Warn($"Administrator {admin.Name} muted {muteResponse.Name} for {duration} minutes");
			// });
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

			long targetId;

			try
			{
				targetId = long.Parse(playerId);
			}
			catch (Exception)
			{
				admin.SendChatMessage(UnMuteUsage);
				return;
			}

			// TODO

			// UnmuteResponse unmuteResponse;

			// try
			// {
			// 	unmuteResponse = await Infrastructure.RpcClients.PlayerService.UnmuteAsync(new UnmuteRequest(targetId, admin.StaticId));
			// }
			// catch (Exception)
			// {
			// 	NAPI.Task.Run(() => admin.SendChatMessage($"Пользователь со static ID {targetId} не найден, либо мут отсутствует"));
			// 	return;
			// }

			// NAPI.Task.Run(() =>
			// {
			// 	CPlayer targetPlayer = PlayerUtil.GetByStaticId(targetId);
			// 	if (targetPlayer != null)
			// 	{
			// 		targetPlayer.MuteState = new MuteState();
			// 		VoiceChatController.Unmute(targetPlayer);
			// 	}

			// 	Chat.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} снял мут {unmuteResponse.Name}");
			// 	this.Logger.Warn($"Administrator {admin.Name} unmuted {unmuteResponse.Name}");
			// });
		}
	}
}
