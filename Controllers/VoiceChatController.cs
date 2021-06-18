using Gamemode.Models.Player;
using Gamemode.Services.Player;
using GTANetworkAPI;

namespace Gamemode.Controllers
{
    public class VoiceChatController : Script
    {
        [RemoteEvent("add_voice_listener")]
        public void AddVoiceListener(CustomPlayer player, CustomPlayer target)
        {
            if (target == null || !target.Exists || !ChatService.CheckMute(player)) return;

            player.EnableVoiceTo(target);
        }

        [RemoteEvent("remove_voice_listener")]
        public void RemoveVoiceListener(CustomPlayer player, CustomPlayer target)
        {
            player.DisableVoiceTo(target);
        }
    }
}