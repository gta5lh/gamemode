// <copyright file="ChatController.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Game.Chat.Events
{
	using System.Threading.Tasks;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class Chat : Script
	{
		[ServerEvent(Event.ChatMessage)]
		private async Task ChatMessage(CPlayer player, string message)
		{
			if (await CheckMute(player)) return;

			NAPI.Task.Run(() =>
			{
				NAPI.Chat.SendChatMessageToAll(string.Format("{0}{1} [{2}]:~s~ {3}", player.ChatColor, player.Name, player.Id, message));
			});
		}

		public static async Task<bool> CheckMute(CPlayer player)
		{
			bool isMuted = player.MuteState != null && player.MuteState.IsMuted();
			if (isMuted && player.MuteState!.HasMuteExpired())
			{
				await player.Unmute();
				NAPI.Task.Run(() =>
				{
					// VoiceChatController.Unmute(player);
					player.SendChatMessage("Срок действия вашего мута истек. Не нарушайте правила сервера. Приятной игры!");
				});

				isMuted = false;
			}
			else if (isMuted)
			{
				player.SendChatMessage($"Администратор выдал вам мут. Осталось {player.MuteState!.GetMinutesLeft():0.##} минут.");
			}

			return isMuted;
		}
	}
}
