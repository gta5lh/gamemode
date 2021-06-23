using Gamemode.Models.Player;
using GTANetworkAPI;
using System;

namespace Gamemode.Commands.Player
{
	public class CMuteCommand : Script
	{
		private const string CMuteCommandUsage = "Использование: /cmute {player_id}. Пример: /cmute 10";

		[Command("cmute", CMuteCommandUsage, Alias = "cm", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		public void CMute(CustomPlayer player, string targetIdInput = null)
		{
			if (targetIdInput == null)
			{
				player.SendChatMessage(CMuteCommandUsage);
				return;
			}

			ushort targetId = 0;

			try
			{
				targetId = ushort.Parse(targetIdInput);
			}
			catch (Exception)
			{
				player.SendChatMessage(CMuteCommandUsage);
				return;
			}

			CustomPlayer targetPlayer = PlayerUtil.GetById(targetId);
			if (targetPlayer == null)
			{
				player.SendChatMessage($"Пользователь с DID {targetId} не найден");
				return;
			}

			if (targetPlayer == player)
			{
				player.SendChatMessage("Нельзя замутить самого себя");
				return;
			}

			player.TriggerEvent("mute", targetPlayer.Id);
		}

		private const string CUnmuteCommandUsage = "Использование: /cunmute {player_id}. Пример: /cunmute 10";

		[Command("cunmute", CUnmuteCommandUsage, Alias = "cum", SensitiveInfo = true, GreedyArg = true, Hide = true)]
		public void CUnmute(CustomPlayer player, string targetIdInput = null)
		{
			if (targetIdInput == null)
			{
				player.SendChatMessage(CUnmuteCommandUsage);
				return;
			}

			ushort targetId = 0;

			try
			{
				targetId = ushort.Parse(targetIdInput);
			}
			catch (Exception)
			{
				player.SendChatMessage(CUnmuteCommandUsage);
				return;
			}

			CustomPlayer targetPlayer = PlayerUtil.GetById(targetId);
			if (targetPlayer == null)
			{
				player.SendChatMessage($"Пользователь с DID {targetId} не найден");
				return;
			}

			player.TriggerEvent("unmute", targetPlayer.Id);
		}
	}
}