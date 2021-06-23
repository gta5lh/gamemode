using Gamemode.Models.Admin;
using Gamemode.Models.Player;
using GTANetworkAPI;
using System;

namespace Gamemode.Commands.Admin
{
	public class SpectateCommand : BaseCommandHandler
	{
		private const string SpectateCommandUsage = "Использование: /spectate {player_id}. Пример: /spectate 10";

		[Command("spectate", SpectateCommandUsage, Alias = "spec", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void Spectate(CustomPlayer admin, string targetIdInput = null)
		{
			if (targetIdInput == null)
			{
				admin.SendChatMessage(SpectateCommandUsage);
				return;
			}

			ushort targetId = 0;

			try
			{
				targetId = ushort.Parse(targetIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(SpectateCommandUsage);
				return;
			}

			CustomPlayer targetPlayer = PlayerUtil.GetById(targetId);
			if (targetPlayer == null)
			{
				admin.SendChatMessage($"Пользователь с DID {targetId} не найден");
				return;
			}

			if (targetPlayer == admin)
			{
				admin.SendChatMessage("Нельзя начать слежение за самим собой");
				return;
			}

			if (admin.SpectatePosition == null) admin.SpectatePosition = admin.Position;
			admin.Spectating = true;
			admin.RemoveAllWeapons();
			admin.Position = targetPlayer.Position + new Vector3(0, 0, 10);
			admin.Dimension = targetPlayer.Dimension;
			admin.TriggerEvent("spectate", targetPlayer.Id);

			AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} начал следить за {targetPlayer.Name}");
			this.Logger.Warn($"Administrator {admin.Name} spectate {targetPlayer.Name}");
		}

		private const string SpectateStopCommandUsage = "Использование: /spectatestop. Пример: /specstop";

		[AdminMiddleware(AdminRank.Junior)]
		[Command("spectatestop", SpectateStopCommandUsage, Alias = "specstop", GreedyArg = true, Hide = true)]
		public void SpectateStop(CustomPlayer admin)
		{
			if (admin.SpectatePosition == null)
			{
				admin.SendChatMessage("Вы не в режиме слежения");
				return;
			}
			admin.Position = admin.SpectatePosition;
			admin.Dimension = 0;
			admin.Spectating = false;
			admin.SpectatePosition = null;
			admin.TriggerEvent("spectateStop");
		}
	}
}
