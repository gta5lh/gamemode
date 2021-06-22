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

			if (!admin.HasData("isSpectating")) admin.SetData("isSpectating", admin.Position);
			admin.Transparency = 0;
			admin.Position = targetPlayer.Position + new Vector3(0, 0, 5);
			admin.TriggerEvent("spectate", targetPlayer.Id);

			AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} начал следить за {targetPlayer.Name}");
			this.Logger.Warn($"Administrator {admin.Name} spectate {targetPlayer.Name}");
		}

		private const string SpectateStopCommandUsage = "Использование: /spectatestop. Пример: /specstop";

		[AdminMiddleware(AdminRank.Junior)]
		[Command("spectatestop", SpectateStopCommandUsage, Alias = "specstop", GreedyArg = true, Hide = true)]
		public void SpectateStop(CustomPlayer admin)
		{
			admin.Position = admin.GetData<Vector3>("isSpectating");
			admin.Transparency = 255;
			admin.ResetData("isSpectating");
			admin.TriggerEvent("spectateStop");
		}
	}
}
