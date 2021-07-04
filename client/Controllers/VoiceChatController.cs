using GamemodeClient.Utils;
using RAGE;
using RAGE.Elements;
using System.Collections.Generic;
using RAGE.Ui;

namespace GamemodeClient.Controllers
{
	public class VoiceChatController : Events.Script
	{
		private const bool Use3d = true;
		private const float MaxRange = 25.0f;

		private List<RAGE.Elements.Player> Listeners = new List<RAGE.Elements.Player>();
		private List<RAGE.Elements.Player> MutedPlayers = new List<RAGE.Elements.Player>();
		private bool LastKeyState = false;
		private long LastTime = 0;
		private bool Muted = false;

		public VoiceChatController()
		{
			Events.Tick += OnTick;
			Events.OnPlayerQuit += OnPlayerQuit;
			Events.OnEntityStreamOut += OnEntityStreamOut;

			Events.Add("muted", OnMuted);
			Events.Add("mute", OnMute);
			Events.Add("unmute", OnUnmute);
		}
		public void OnTick(List<Events.TickNametagData> nametags)
		{
			foreach (RAGE.Elements.Player player in Entities.Players.Streamed)
			{
				if (player == RAGE.Elements.Player.LocalPlayer || MutedPlayers.Contains(player)) continue;

				object isSpeaking = player.GetSharedData("isSpeaking");
				if (isSpeaking == null || !(bool)isSpeaking) continue;

				float dist = Player.CurrentPlayer.Position.DistanceTo(player.Position);
				if (dist > MaxRange) continue;

				Vector3 targetPlayerPosition = player.Position + new Vector3(0, 0, 0.99f);

				float x = 0;
				float y = 0;

				LoadIcon();
				RAGE.Game.Graphics.GetScreenCoordFromWorldCoord(targetPlayerPosition.X, targetPlayerPosition.Y, targetPlayerPosition.Z, ref x, ref y);
				RAGE.Game.Graphics.DrawSprite("mpleaderboard", "leaderboard_audio_3", x, y, 0.025f - (dist / MaxRange * 0.020f), 0.05f - (dist / MaxRange * 0.035f), 0, 255, 255, 255, 255, 0);
			}

			bool speakingKeyPressed = Input.IsDown(RAGE.Ui.VirtualKeys.Z) && !RAGE.Game.Ui.IsTextChatActive();

			if (speakingKeyPressed && !this.Muted)
			{
				LoadIcon();
				RAGE.Game.Graphics.DrawSprite("mpleaderboard", "leaderboard_audio_3", Minimap.GetMinimapAnchor().right_x + 0.01f, Minimap.GetMinimapAnchor().bottom_y - 0.035f, 0.025f, 0.05f, 0, 255, 255, 255, 255, 0);
			}

			if (speakingKeyPressed && !this.LastKeyState)
			{
				Voice.Muted = false;
				Events.CallRemote("start_voice");
			}
			else if (!speakingKeyPressed && this.LastKeyState)
			{
				Voice.Muted = true;
				Events.CallRemote("stop_voice");
			}

			this.LastKeyState = speakingKeyPressed;

			long currentTime = Time.GetCurTimestamp();
			if (currentTime - this.LastTime < 250) return;
			this.LastTime = currentTime;

			foreach (RAGE.Elements.Player player in Entities.Players.Streamed)
			{
				if (this.Listeners.Contains(player))
				{
					float dist = RAGE.Elements.Player.LocalPlayer.Position.DistanceTo(player.Position);

					if (dist < MaxRange)
					{
						if (!MutedPlayers.Contains(player)) player.VoiceVolume = 2.0f - (dist / MaxRange);
						else if (player.VoiceVolume > 0) player.VoiceVolume = 0;
					}
					else
					{
						this.RemoveListener(player, true);
					}
				}
				else
				{
					if (RAGE.Elements.Player.LocalPlayer.Position.DistanceTo(player.Position) > MaxRange
						|| player == RAGE.Elements.Player.LocalPlayer) continue;
					this.AddListener(player);
				}
			}
		}

		public void OnMuted(object[] args)
		{
			this.Muted = (bool)args[0];
		}

		public void OnMute(object[] args)
		{
			int targetId = (int)args[0];

			RAGE.Elements.Player found = Entities.Players.All.Find(x => x.RemoteId == targetId);
			if (found == null)
			{
				Chat.Output($"Игрок с {targetId} ID не найден");
				return;
			}

			this.MutedPlayers.Add(found);
			Chat.Output($"Игрок с {targetId} ID заглушён");
		}

		public void OnUnmute(object[] args)
		{
			int targetId = (int)args[0];

			RAGE.Elements.Player found = Entities.Players.All.Find(x => x.RemoteId == targetId);
			if (found == null)
			{
				Chat.Output($"Игрок с {targetId} ID не найден");
				return;
			}

			if (!this.MutedPlayers.Contains(found))
			{
				Chat.Output($"Игрок с {targetId} ID не заглушён");
				return;
			}

			this.MutedPlayers.Remove(found);
			Chat.Output($"Игроку с { targetId } ID снята заглушка");
		}

		public void OnEntityStreamOut(Entity entity)
		{
			if (entity.Type != RAGE.Elements.Type.Player) return;

			RAGE.Elements.Player player = (RAGE.Elements.Player)entity;
			if (this.Listeners.Contains(player)) this.RemoveListener(player, true);
		}

		public void OnPlayerQuit(RAGE.Elements.Player player)
		{
			if (this.Listeners.Contains(player)) this.RemoveListener(player, false);
		}

		private void AddListener(RAGE.Elements.Player player)
		{
			this.Listeners.Add(player);
			Events.CallRemote("add_voice_listener", player);

			player.Voice3d = Use3d;
		}

		private void RemoveListener(RAGE.Elements.Player player, bool notify)
		{
			this.Listeners.Remove(player);

			if (notify)
			{
				Events.CallRemote("remove_voice_listener", player);
			}
		}

		private void LoadIcon()
		{
			if (!RAGE.Game.Graphics.HasStreamedTextureDictLoaded("mpleaderboard"))
			{
				RAGE.Game.Graphics.RequestStreamedTextureDict("mpleaderboard", true);
			}
		}
	}
}
