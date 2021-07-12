using System.Threading.Tasks;
using Gamemode.Models.Player;
using Gamemode.Services.Player;
using GamemodeCommon.Models.Data;
using GTANetworkAPI;

namespace Gamemode.Controllers
{
	public class VoiceChatController : Script
	{
		private const float MaxRange = 25.0f;

		[RemoteEvent("start_voice")]
		public async Task StartVoice(CustomPlayer player)
		{
			bool muted = await ChatService.CheckMute(player);

			NAPI.Task.Run(() =>
			{
				if (!muted)
				{
					if (!player.Noclip && !player.Spectating && !player.Invisible)
						player.SetSharedData(DataKey.IsSpeaking, true);

					player.TriggerEvent("muted", false);
				}
				else
				{
					player.TriggerEvent("muted", true);
				}
			});
		}

		[RemoteEvent("stop_voice")]
		public void StopVoice(CustomPlayer player)
		{
			player.SetSharedData(DataKey.IsSpeaking, false);
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
			targetPlayer.SetSharedData(DataKey.IsSpeaking, false);
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
