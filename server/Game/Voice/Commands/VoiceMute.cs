// <copyright file="VoiceMute.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Voice.Commands
{
	using System;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class VoiceMute : Script
	{
		private const string VoiceMuteUsage = "Использование: /vm {player_id}. Пример: /vm 10";
		private const string VoiceUnmuteUsage = "Использование: /vum {player_id}. Пример: /vum 10";

		[Command("voicemute", VoiceMuteUsage, Alias = "vm", GreedyArg = true)]
		public static void OnVoiceMute(CPlayer player, string? targetIdInput = null)
		{
			if (targetIdInput == null)
			{
				player.SendChatMessage(VoiceMuteUsage);
				return;
			}

			ushort targetId;
			try
			{
				targetId = ushort.Parse(targetIdInput);
			}
			catch (Exception)
			{
				player.SendChatMessage(VoiceMuteUsage);
				return;
			}

			CPlayer? targetPlayer = Gamemode.Game.Player.Util.GetById(targetId);
			if (targetPlayer == null)
			{
				player.SendChatMessage($"Пользователь с {targetId} ID не найден");
				return;
			}

			if (targetPlayer == player)
			{
				player.SendChatMessage("Нельзя заглушить самого себя");
				return;
			}

			player.TriggerEvent("mute", targetPlayer.Id);
		}

		[Command("voiceunmute", VoiceUnmuteUsage, Alias = "vum", GreedyArg = true)]
		public static void VoiceUnmute(CPlayer player, string? targetIdInput = null)
		{
			if (targetIdInput == null)
			{
				player.SendChatMessage(VoiceUnmuteUsage);
				return;
			}

			ushort targetId;
			try
			{
				targetId = ushort.Parse(targetIdInput);
			}
			catch (Exception)
			{
				player.SendChatMessage(VoiceUnmuteUsage);
				return;
			}

			CPlayer targetPlayer = Gamemode.Game.Player.Util.GetById(targetId);
			if (targetPlayer == null)
			{
				player.SendChatMessage($"Пользователь с {targetId} ID не найден");
				return;
			}

			player.TriggerEvent("unmute", targetPlayer.Id);
		}
	}
}
