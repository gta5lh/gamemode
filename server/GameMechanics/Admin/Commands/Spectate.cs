using GTANetworkAPI;
using System;
using Gamemode.GameMechanics.Admin.Models;
using Gamemode.GameMechanics.Player.Models;

namespace Gamemode.GameMechanics.Admin.Commands
{
	public class SpectateCommand : BaseHandler
	{
		private const string SpectateUsage = "Использование: /spectate {player_id}. Пример: /spectate 10";

		[Command("spectate", SpectateUsage, Alias = "spec", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void OnSpectate(CPlayer admin, string? targetIdInput = null)
		{
			if (targetIdInput == null)
			{
				admin.SendChatMessage(SpectateUsage);
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
				admin.SendChatMessage(SpectateUsage);
				return;
			}

			// TODO
			// CPlayer targetPlayer = PlayerUtil.GetById(targetId);
			// if (targetPlayer == null)
			// {
			// 	admin.SendChatMessage($"Пользователь с DID {targetId} не найден");
			// 	return;
			// }

			// if (targetPlayer == admin)
			// {
			// 	admin.SendChatMessage("Нельзя начать слежение за самим собой");
			// 	return;
			// }

			// SpectateController.StartSpectate(admin, targetPlayer);

			// AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} начал следить за {targetPlayer.Name}");
			// this.Logger.Warn($"Administrator {admin.Name} started spectate {targetPlayer.Name}");
		}

		private const string SpectateStopUsage = "Использование: /spectatestop. Пример: /specstop";

		[AdminMiddleware(AdminRank.Junior)]
		[Command("spectatestop", SpectateStopUsage, Alias = "specstop", GreedyArg = true, Hide = true)]
		public void OnSpectateStop(CPlayer admin)
		{
			// TODO
			// if (admin.SpectatePosition == null)
			// {
			// 	admin.SendChatMessage("Вы не в режиме слежения");
			// 	return;
			// }

			// SpectateController.StopSpectate(admin);
		}
	}
}
