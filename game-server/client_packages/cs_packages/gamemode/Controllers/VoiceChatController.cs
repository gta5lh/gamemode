// <copyright file="VoiceChatController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace GamemodeClient.Controllers
{
	using GamemodeClient.Utils;
	using RAGE;
	using RAGE.Elements;
	using System.Collections.Generic;
	using RAGE.Ui;
	using GamemodeCommon.Models.Data;
	using System;

	public class VoiceChatController : Events.Script
	{
		public delegate void playerVoiceStateChangedDelegate(bool enabled);
		public static event playerVoiceStateChangedDelegate playerVoiceStateChangedEvent;

		private const bool Use3d = true;
		private const float MaxRange = 25 * 25;
		private const float MaxDist = 25.0f;

		private List<RAGE.Elements.Player> Listeners = new List<RAGE.Elements.Player>();
		private List<RAGE.Elements.Player> MutedPlayers = new List<RAGE.Elements.Player>();
		private long LastTime = 0;
		private long LastTimePressed = 0;
		private bool Muted = false;

		public VoiceChatController()
		{
			Events.Tick += this.OnTick;
			Events.OnPlayerQuit += this.OnPlayerQuit;
			Events.OnEntityStreamOut += this.OnEntityStreamOut;

			Events.Add("muted", this.OnMuted);
			Events.Add("mute", this.OnMute);
			Events.Add("unmute", this.OnUnmute);
		}
		public void OnTick(List<Events.TickNametagData> nametags)
		{
			if (nametags != null)
			{
				int screenX = 0, screenY = 0;
				RAGE.Game.Graphics.GetScreenResolution(ref screenX, ref screenY);

				foreach (Events.TickNametagData nametag in nametags)
				{
					if (nametag.Player == RAGE.Elements.Player.LocalPlayer || this.MutedPlayers.Contains(nametag.Player))
					{
						continue;
					}

					object isSpeaking = nametag.Player.GetSharedData(DataKey.IsSpeaking);
					if (isSpeaking == null || !(bool)isSpeaking)
					{
						continue;
					}

					if (nametag.Distance > MaxRange)
					{
						continue;
					}

					Vector3 targetPlayerPosition = nametag.Player.Position + new Vector3(0, 0, 1.30f);

					float x = 0;
					float y = 0;

					this.LoadIcon();
					RAGE.Game.Graphics.GetScreenCoordFromWorldCoord(targetPlayerPosition.X, targetPlayerPosition.Y, targetPlayerPosition.Z, ref x, ref y);
					float dist = Player.CurrentPlayer.Position.DistanceTo(nametag.Player.Position);
					RAGE.Game.Graphics.DrawSprite("mpleaderboard", "leaderboard_audio_3", x, y, 0.025f - (dist / MaxDist * 0.020f), 0.05f - (dist / MaxDist * 0.035f), 0, 255, 255, 255, 255, 0);
				}
			}

			bool speakingKeyPressed = Input.IsDown(RAGE.Ui.VirtualKeys.N) && !RAGE.Ui.Cursor.Visible;

			long currentTime = Time.GetCurTimestamp();

			if (speakingKeyPressed)
			{
				this.LastTimePressed = currentTime;

				if (Voice.Muted)
				{
					Voice.Muted = false;
					Events.CallRemote("start_voice");
					playerVoiceStateChangedEvent(!Voice.Muted);
				}
			}
			else if (!speakingKeyPressed && !Voice.Muted)
			{
				if (currentTime - this.LastTimePressed > 500)
				{
					Voice.Muted = true;
					Events.CallRemote("stop_voice");
					playerVoiceStateChangedEvent(!Voice.Muted);
				}
			}

			if (currentTime - this.LastTime < 250)
			{
				return;
			}
			this.LastTime = currentTime;

			foreach (RAGE.Elements.Player player in Entities.Players.Streamed)
			{
				if (this.Listeners.Contains(player))
				{
					float dist = RAGE.Elements.Player.LocalPlayer.Position.DistanceTo(player.Position);

					if (dist < MaxRange)
					{
						if (!this.MutedPlayers.Contains(player))
						{
							player.VoiceVolume = 2.0f - (dist / MaxRange);
						}
						else if (player.VoiceVolume > 0)
						{
							player.VoiceVolume = 0;
						}
					}
					else
					{
						this.RemoveListener(player, true);
					}
				}
				else
				{
					if (RAGE.Elements.Player.LocalPlayer.Position.DistanceTo(player.Position) > MaxRange
						|| player == RAGE.Elements.Player.LocalPlayer)
					{
						continue;
					}
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
			if (entity.Type != RAGE.Elements.Type.Player)
			{
				return;
			}

			RAGE.Elements.Player player = (RAGE.Elements.Player)entity;
			if (this.Listeners.Contains(player))
			{
				this.RemoveListener(player, true);
			}
		}

		public void OnPlayerQuit(RAGE.Elements.Player player)
		{
			if (this.Listeners.Contains(player))
			{
				this.RemoveListener(player, false);
			}
		}

		private async void AddListener(RAGE.Elements.Player player)
		{
			bool added = Convert.ToBoolean(await Events.CallRemoteProc("add_voice_listener", player));
			if (added)
			{
				this.Listeners.Add(player);
				player.Voice3d = Use3d;
			}
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
