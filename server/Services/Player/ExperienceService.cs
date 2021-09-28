using System.Threading.Tasks;
using Gamemode.Models.Player;
using GamemodeCommon.Models;
using GTANetworkAPI;

namespace Gamemode.Services.Player
{
	public class ExperienceService
	{
		public static async Task OnPlayerDeathAsync(CustomPlayer target, CustomPlayer killer, uint reason)
		{
			short delta = killer.Fraction == target.Fraction ? (short)-1 : (short)1;

			await ChangeExperience(killer, delta);
		}

		public static async Task ChangeExperience(CustomPlayer player, short delta)
		{
			long previousExperience = player.CurrentExperience;
			player.CurrentExperience += delta;

			NAPI.ClientEvent.TriggerClientEvent(player, "ExperienceChanged", previousExperience, player.CurrentExperience, player.RequiredExperience, player.FractionRank);

			if (player.CurrentExperience >= player.RequiredExperience && player.FractionRank < 10)
			{
				await player.RankUp();

				NAPI.Task.Run(() =>
				{
					if (player.FractionRank < 10)
					{
						NAPI.ClientEvent.TriggerClientEvent(player, "ExperienceChanged", (long)0, player.CurrentExperience, player.RequiredExperience);
					}

					NAPI.ClientEvent.TriggerClientEvent(player, "RankedUp", player.FractionRank, player.FractionRankName);
					player.SendNotification($"Повысился до ранга {player.FractionRankName} [{player.FractionRank}]", 1500, 5000, NotificationType.Success);
				});
			}
			else if (player.CurrentExperience < 0 && player.FractionRank > 1)
			{
				await player.RankDown();

				NAPI.Task.Run(() =>
				{
					NAPI.ClientEvent.TriggerClientEvent(player, "ExperienceChanged", (long)0, player.CurrentExperience, player.RequiredExperience);
					NAPI.ClientEvent.TriggerClientEvent(player, "RankedDown", player.FractionRank, player.FractionRankName);
					player.SendNotification($"Понизился до ранга {player.FractionRankName} [{player.FractionRank}]", 1500, 5000, NotificationType.Error);
				});
			}
		}
	}
}
