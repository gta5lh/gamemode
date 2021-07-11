using Gamemode.Controllers;
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

			if (admin.Noclip)
			{
				admin.SendChatMessage("Нельзя начать слежение в режиме Noclip");
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

			SpectateController.StartSpectate(admin, targetPlayer);

			AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} начал следить за {targetPlayer.Name}");
			this.Logger.Warn($"Administrator {admin.Name} started spectate {targetPlayer.Name}");
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

			SpectateController.StopSpectate(admin);
		}
	}
}
