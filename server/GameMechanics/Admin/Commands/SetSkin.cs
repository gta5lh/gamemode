using System;
using GTANetworkAPI;
using Gamemode.GameMechanics.Admin.Models;
using Gamemode.GameMechanics.Player.Models;

namespace Gamemode.GameMechanics.Admin.Commands
{
	public class SetSkin : BaseHandler
	{
		private const string SetSkinUsage = "Использование: /setskin {dynamic_id} {skin}. Пример: /setskin 10 1";

		[Command("setskin", SetSkinUsage, Alias = "sk", GreedyArg = true, Hide = true)]
		[AdminMiddleware(AdminRank.Senior)]
		public void OnSetSkin(CPlayer admin, string? playerIdInput = null, string? skin = null)
		{
			if (playerIdInput == null || skin == null)
			{
				admin.SendChatMessage(SetSkinUsage);
				return;
			}

			ushort playerId;

			try
			{
				playerId = ushort.Parse(playerIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(SetSkinUsage);
				return;
			}

			uint pedHash = NAPI.Util.GetHashKey(skin);
			if (pedHash == 0)
			{
				admin.SendChatMessage($"Скин с названием {skin} отсутствует");
				return;
			}

			// TODO
			// CPlayer targetPlayer = PlayerUtil.GetById(playerId);
			// if (targetPlayer == null)
			// {
			// 	admin.SendChatMessage($"Пользователь с DID {playerId} не найден");
			// 	return;
			// }

			// targetPlayer.SetSkin(pedHash);

			// AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} сменил скин {targetPlayer.Name} на {skin}");
			// this.Logger.Warn($"Administrator {admin.Name} set skin of {targetPlayer.Name} to {skin}");
		}
	}
}
