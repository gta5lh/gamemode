using System;
using Gamemode.Mechanics.Admin.Models;
using Gamemode.Mechanics.Chat;
using Gamemode.Mechanics.Player.Models;
using Gamemode.Mechanics.Utils;
using GTANetworkAPI;

namespace Gamemode.Mechanics.Admin.Commands
{
	public class Kick : BaseHandler
	{
		private const string KickUsage = "Использование: /kick {player_id} {причина}. Пример: /kick 1 Бот";

		[Command("kick", KickUsage, SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void Ban(CPlayer admin, string? targetIdInput = null, string? reason = null)
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
