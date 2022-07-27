using System;
using Gamemode.GameMechanics.Admin.Models;
using Gamemode.GameMechanics.Player.Models;
using GTANetworkAPI;

namespace Gamemode.GameMechanics.Admin.Commands
{
	public class Kick : BaseHandler
	{
		private const string KickUsage = "Использование: /kick {player_id} {причина}. Пример: /ban 1 Бот";

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

			// TODO
			// CPlayer targetPlayer = PlayerUtil.GetById(targetId);
			// if (targetPlayer == null || targetPlayer.AdminRank != 0)
			// {
			// 	admin.SendChatMessage($"Пользователь с DID {targetId} не найден");
			// 	return;
			// }

			// targetPlayer.Kick();
			// Chat.SendColorizedChatMessageToAll(ChatColor.AdminAnnouncementColor, $"Администратор: {admin.Name} кикнул {targetPlayer.Name}. Причина: {reason}");
			// this.Logger.Warn($"Administrator {admin.Name} kicked {targetPlayer.Name}");
		}
	}
}
