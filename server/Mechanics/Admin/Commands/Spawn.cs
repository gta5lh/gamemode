using System;
using GTANetworkAPI;
using Gamemode.Mechanics.Admin.Models;
using Gamemode.Mechanics.Player.Models;

namespace Gamemode.Mechanics.Admin.Commands
{
	public class Spawn : BaseHandler
	{
		private const string SpawnUsage = "Использование: /spawn {player_id}. Пример: /sp 0";

		[Command("spawnplayer", SpawnUsage, Alias = "sp", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Junior)]
		public void OnSpawn(CPlayer admin, string? playerIdInput = null)
		{
			if (playerIdInput == null)
			{
				admin.SendChatMessage(SpawnUsage);
				return;
			}

			ushort playerId;

			try
			{
				playerId = ushort.Parse(playerIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(SpawnUsage);
				return;
			}

			CPlayer? targetPlayer = PlayerUtil.GetById(playerId);
			if (targetPlayer == null)
			{
				admin.SendChatMessage($"Пользователь с DID {playerId} не найден");
				return;
			}

			NAPI.Player.SpawnPlayer(targetPlayer, new Vector3(0, 0, 0));
			Cache.SendMessageToAllAdminsAction($"{admin.Name} заспавнил {targetPlayer.Name}");
			this.Logger.Warn($"Administrator {admin.Name} spawned {targetPlayer.Name}");
		}
	}
}
