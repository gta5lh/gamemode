using System;
using System.Threading.Tasks;
using Gamemode.Models.Player;
using Gamemode.Services;
using GTANetworkAPI;
using Gamemode.Cache.Player;

namespace Gamemode.Commands.Admin
{
	public class SetSkinCommand : BaseCommandHandler
	{
		private const string SetSkinCommandUsage = "Использование: /setskin {dynamic_id} {skin}. Пример: /setskin 10 1";

		[Command("setskin", SetSkinCommandUsage, Alias = "sk", GreedyArg = true, Hide = true)]
		[AdminMiddleware(Models.Admin.AdminRank.Senior)]
		public void SetSkin(CustomPlayer admin, string playerIdInput = null, string skin = null)
		{
			if (playerIdInput == null || skin == null)
			{
				admin.SendChatMessage(SetSkinCommandUsage);
				return;
			}

			ushort playerId;

			try
			{
				playerId = ushort.Parse(playerIdInput);
			}
			catch (Exception)
			{
				admin.SendChatMessage(SetSkinCommandUsage);
				return;
			}

			uint pedHash = NAPI.Util.GetHashKey(skin);
			if (pedHash == 0)
			{
				admin.SendChatMessage($"Скин с названием {skin} отсутствует");
				return;
			}

			CustomPlayer targetPlayer = PlayerUtil.GetById(playerId);
			if (targetPlayer == null)
			{
				admin.SendChatMessage($"Пользователь с DID {playerId} не найден");
				return;
			}

			targetPlayer.SetSkin(pedHash);

			AdminsCache.SendMessageToAllAdminsAction($"{admin.Name} сменил скин {targetPlayer.Name} на {skin}");
			this.Logger.Warn($"Administrator {admin.Name} set skin of {targetPlayer.Name} to {skin}");
		}
	}
}
