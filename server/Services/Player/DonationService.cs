using System.Threading.Tasks;
using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Services.Player
{
	public class DonationService
	{
		public static void DisplayDonationNotification(long playerID, long amount)
		{
			NAPI.Task.Run(() =>
			{
				CustomPlayer targetPlayer = PlayerUtil.GetByStaticId(playerID);
				if (targetPlayer == null)
				{
					return;
				}

				NAPI.ClientEvent.TriggerClientEvent(targetPlayer, "DisplayDonationNotification", amount);
			});
		}
	}
}
