using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Controllers
{
    public class VoiceChatController : Script
    {
        [RemoteEvent("add_voice_listener")]
        public void AddVoiceListener(CustomPlayer player, CustomPlayer target)
        {
            if (player.HasData("mute")) return;
            player.EnableVoiceTo(target);
        }

        [RemoteEvent("remove_voice_listener")]
        public void RemoveVoiceListener(CustomPlayer player, CustomPlayer target)
        {
            player.DisableVoiceTo(target);
        }
    }
}