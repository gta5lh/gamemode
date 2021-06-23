using System;
using System.Threading.Tasks;
using Gamemode.ApiClient.Models;
using Gamemode.Models.Player;
using GTANetworkAPI;

namespace Gamemode.Services
{
	public static class GangService
	{
		public static async Task<string?> SetAsGangMember(CustomPlayer player, long playerId, byte gangId, byte tier, long setBy)
		{
			SetFractionResponse setFractionResponse;

			try
			{
				setFractionResponse = await ApiClient.ApiClient.SetFraction(playerId, gangId, tier, setBy);
			}
			catch (Exception e)
			{
				throw e;
			}

			if (player == null)
			{
				return setFractionResponse.Name;
			}

			NAPI.Task.Run(() =>
			{
				player.Fraction = gangId == 0 ? null : (byte?)gangId;
				player.FractionRank = tier == 0 ? null : (byte?)tier;
				player.FractionRankName = setFractionResponse.TierName;
				player.RequiredExperience = setFractionResponse.TierRequiredExperience;
				player.CurrentExperience = 0;
				player.SetSkin(gangId == 0 ? PedHash.Tramp01 : (PedHash)setFractionResponse.Skin);
				player.CustomRemoveAllWeapons();

				if (gangId != 0)
				{
					foreach (Weapon weapon in GangUtil.WeaponsByGangId[gangId])
					{
						player.CustomGiveWeapon(weapon.Hash, weapon.Amount);
					}
				}
			});

			return setFractionResponse.Name;
		}
	}
}
