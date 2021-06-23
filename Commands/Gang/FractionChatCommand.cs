using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Commands.Gang
{
	public class FractionChatCommand : Script
	{
		private const string FractionChatCommandUsage = "Использование: /f";

		[Command("f", FractionChatCommandUsage, GreedyArg = true)]
		public void ReportAnswer(CustomPlayer player, string message = null)
		{
			if (message == null)
			{
				player.SendChatMessage(FractionChatCommandUsage);
				return;
			}

			if (player.Fraction == null)
			{
				player.SendChatMessage("Ты не являешься членом фракции");
				return;
			}

			FractionsCache.SendMessageToAllFractionMembers((byte)player.Fraction, $"{player.FractionRankName} {player.Name}: {message}");
		}
	}
}
