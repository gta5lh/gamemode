using Gamemode.Models.Player;
using GTANetworkAPI;
using System;

namespace Gamemode.Commands.Player
{
	public class VoiceMuteCommand : Script
	{
		private const string VoiceMuteCommandUsage = "Использование: /vm {player_id}. Пример: /vm 10";

		[Command("voicemute", VoiceMuteCommandUsage, Alias = "vm", GreedyArg = true)]
		public void VoiceMute(CustomPlayer player, string targetIdInput = null)
		{
			if (targetIdInput == null)
			{
				player.SendChatMessage(VoiceMuteCommandUsage);
				return;
			}

			ushort targetId = 0;

			try
			{
				targetId = ushort.Parse(targetIdInput);
			}
			catch (Exception)
			{
				player.SendChatMessage(VoiceMuteCommandUsage);
				return;
			}

			CustomPlayer targetPlayer = PlayerUtil.GetById(targetId);
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

		private const string VoiceUnmuteCommandUsage = "Использование: /vum {player_id}. Пример: /vum 10";

		[Command("voiceunmute", VoiceUnmuteCommandUsage, Alias = "vum", GreedyArg = true)]
		public void VoiceUnmute(CustomPlayer player, string targetIdInput = null)
		{
			if (targetIdInput == null)
			{
				player.SendChatMessage(VoiceUnmuteCommandUsage);
				return;
			}

			ushort targetId = 0;

			try
			{
				targetId = ushort.Parse(targetIdInput);
			}
			catch (Exception)
			{
				player.SendChatMessage(VoiceUnmuteCommandUsage);
				return;
			}

			CustomPlayer targetPlayer = PlayerUtil.GetById(targetId);
			if (targetPlayer == null)
			{
				player.SendChatMessage($"Пользователь с {targetId} ID не найден");
				return;
			}

			player.TriggerEvent("unmute", targetPlayer.Id);
		}
	}
}
