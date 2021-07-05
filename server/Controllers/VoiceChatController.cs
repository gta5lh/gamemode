using Gamemode.Models.Player;
using Gamemode.Services.Player;
using GTANetworkAPI;

namespace Gamemode.Controllers
{
	public class VoiceChatController : Script
	{
		private const float MaxRange = 25.0f;

		[RemoteEvent("start_voice")]
		public void StartVoice(CustomPlayer player)
		{
			if (!ChatService.CheckMute(player))
			{
				if (!player.Noclip && !player.Spectating && !player.Invisible)
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
			if (Vector3.Distance(player.Position, target.Position) > MaxRange || (player.MuteState != null && player.MuteState.IsMuted())) return;

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
			targetPlayer.SetSharedData("isSpeaking", false);
			targetPlayer.TriggerEvent("muted", true);

			foreach (CustomPlayer player in NAPI.Player.GetPlayersInRadiusOfPlayer(MaxRange, targetPlayer))
			{
				if (player == targetPlayer) continue;
				targetPlayer.DisableVoiceTo(player);
			}
		}

		public static void Unmute(CustomPlayer targetPlayer)
		{
			targetPlayer.TriggerEvent("muted", false);

			foreach (CustomPlayer player in NAPI.Player.GetPlayersInRadiusOfPlayer(MaxRange, targetPlayer))
			{
				if (player == targetPlayer) continue;
				targetPlayer.EnableVoiceTo(player);
			}
		}
	}
}
