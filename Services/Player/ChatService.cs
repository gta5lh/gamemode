using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Services.Player
{
    public class ChatService
    {
        public static bool CheckMute(CustomPlayer player, bool notify = true)
        {
            bool isMuted = player.MuteState != null && player.MuteState.IsMuted();
            if (isMuted && player.MuteState.HasMuteExpired())
            {
                player.Unmute();
                if (notify) player.SendChatMessage("Срок действия вашего мута истек. Не нарушайте правила сервера. Приятной игры!");
            }
            else if (isMuted)
            {
                if (notify) player.SendChatMessage($"Администратор выдал вам мут. Осталось {player.MuteState.GetMinutesLeft():0.##} минут.");
            }
            return isMuted;
        }
    }
}
