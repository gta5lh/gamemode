// <copyright file="FractionChat.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Gang.Commands
{
	using Gamemode.Game.Fraction;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public class FractionChat : Script
	{
		private const string FractionChatUsage = "Использование: /f";

		[Command("f", FractionChatUsage, GreedyArg = true)]
		public static void OnFractionChat(CPlayer player, string? message = null)
		{
			if (message == null)
			{
				player.SendChatMessage(FractionChatUsage);
				return;
			}

			if (player.Fraction == null)
			{
				player.SendChatMessage("Ты не являешься членом фракции");
				return;
			}

			Cache.SendMessageToAllFractionMembers((byte)player.Fraction, $"{player.FractionRankName} {player.Name}: {message}");
		}
	}
}
