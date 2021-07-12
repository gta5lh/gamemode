using System.Threading.Tasks;
using Gamemode.Controllers;
using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Services.Player
{
	public class ChatService
	{
		public static async Task<bool> CheckMute(CustomPlayer player)
		{
			bool isMuted = player.MuteState != null && player.MuteState.IsMuted();
			if (isMuted && player.MuteState.HasMuteExpired())
			{
				await player.Unmute();
				NAPI.Task.Run(() =>
				{
					VoiceChatController.Unmute(player);
					player.SendChatMessage("Срок действия вашего мута истек. Не нарушайте правила сервера. Приятной игры!");
				});

				isMuted = false;
			}
			else if (isMuted)
			{
				player.SendChatMessage($"Администратор выдал вам мут. Осталось {player.MuteState.GetMinutesLeft():0.##} минут.");
			}

			return isMuted;
		}
	}
}
