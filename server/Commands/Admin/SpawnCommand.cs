using System;
using Gamemode.Models.Admin;
using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Commands.Admin
{
	public class SpawnCommand : BaseCommandHandler
	{
		private const string SpawnCommandUsage = "Использование: /spawn {player_id}. Пример: /sp 0";

		[Command("spawnplayer", SpawnCommandUsage, Alias = "sp", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void Spawn(CustomPlayer admin, string playerIdInput = null)
		{
			if (playerIdInput == null)
			{
				admin.SendChatMessage(SpawnCommandUsage);
				return;
			}

			ushort playerId;

			try
			{
				playerId = ushort.Parse(playerIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(SpawnCommandUsage);
				return;
			}

			CustomPlayer targetPlayer = PlayerUtil.GetById(playerId);
			if (targetPlayer == null)
			{
				admin.SendChatMessage($"Пользователь с DID {playerId} не найден");
				return;
			}

			NAPI.Player.SpawnPlayer(targetPlayer, new Vector3(0, 0, 0));
			AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} заспавнил {targetPlayer.Name}");
			this.Logger.Warn($"Administrator {admin.Name} spawned {targetPlayer.Name}");
		}
	}
}
