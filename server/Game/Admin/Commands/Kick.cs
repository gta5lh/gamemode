// <copyright file="Kick.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Admin.Commands
{
	using System;
	using Gamemode.Game.Admin.Models;
	using Gamemode.Game.Chat;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class Kick : BaseHandler
	{
		private const string KickUsage = "Использование: /kick {player_id} {причина}. Пример: /kick 1 Бот";

		[Command("kick", KickUsage, SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void OnKick(CPlayer admin, string? targetIdInput = null, string? reason = null)
		{
			if (targetIdInput == null || reason == null)
			{
				admin.SendChatMessage(KickUsage);
				return;
			}

			ushort targetId;

			try
			{
				targetId = ushort.Parse(targetIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(KickUsage);
				return;
			}

			CPlayer targetPlayer = PlayerUtil.GetById(targetId);
			if (targetPlayer == null || targetPlayer.AdminRank != 0)
			{
				admin.SendChatMessage($"Пользователь с DID {targetId} не найден");
				return;
			}

			targetPlayer.Kick();
			ChatColor.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} кикнул {targetPlayer.Name}. Причина: {reason}");
			this.Logger.Warn($"Administrator {admin.Name} kicked {targetPlayer.Name}");
		}
	}
}
