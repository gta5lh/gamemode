using Gamemode.Models.Player;
using Gamemode.Services.Player;
using GTANetworkAPI;

namespace Gamemode.Controllers
{
	public class VoiceChatController : Script
	{
		[RemoteEvent("start_voice")]
		public void StartVoice(CustomPlayer player)
		{
			if (!ChatService.CheckMute(player))
			{
				player.SetSharedData("isSpeaking", true);
				player.TriggerEvent("muted", false);
			}
			else
			{
				player.TriggerEvent("muted", true);
			}
		}

		[RemoteEvent("stop_voice")]
		public void StopVoice(CustomPlayer player)
		{
			player.SetSharedData("isSpeaking", false);
		}

		[RemoteEvent("add_voice_listener")]
		public void AddVoiceListener(CustomPlayer player, CustomPlayer target)
		{
			if (target == null || !target.Exists || ChatService.CheckMute(player, false)) return;
			player.EnableVoiceTo(target);
		}

		[RemoteEvent("remove_voice_listener")]
		public void RemoveVoiceListener(CustomPlayer player, CustomPlayer target)
		{
			if (target == null || !target.Exists) return;
			player.DisableVoiceTo(target);
		}

		public static void Mute(CustomPlayer targetPlayer)
		{
			foreach (CustomPlayer player in NAPI.Player.GetPlayersInRadiusOfPlayer(100, targetPlayer))
			{
				if (player == targetPlayer) continue;
				targetPlayer.DisableVoiceTo(player);
			}
		}

		public static void Unmute(CustomPlayer targetPlayer)
		{
			foreach (CustomPlayer player in NAPI.Player.GetPlayersInRadiusOfPlayer(100, targetPlayer))
			{
				if (player == targetPlayer) continue;
				targetPlayer.EnableVoiceTo(player);
			}
		}
	}
}