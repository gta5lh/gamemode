// <copyright file="GangMgr.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Gang
{
	using System;
	using System.Threading.Tasks;
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;
	using Rpc.Player;

	public static class GangMgr
	{
		public static async Task<string> SetAsGangMember(CPlayer player, string playerStaticId, long gangId, byte tier, Guid setByID)
		{
			SetFractionResponse setFractionResponse = await Infrastructure.RpcClients.PlayerService.SetFractionAsync(new SetFractionRequest(playerStaticId, gangId, tier, setByID));

			if (player == null)
			{
				return setFractionResponse.Name;
			}

			NAPI.Task.Run(() =>
			{
				player.Fraction = gangId == 0 ? null : gangId;
				player.FractionRank = tier == 0 ? null : tier;
				player.FractionRankName = setFractionResponse.TierName?.Length == 0 ? null : setFractionResponse.TierName;
				player.RequiredExperience = setFractionResponse.TierRequiredExperience;
				player.CurrentExperience = 0;
				player.SetSkin(gangId == 0 ? PedHash.Tramp01 : (PedHash)setFractionResponse.Skin);
				player.CustomRemoveAllWeapons();
				NAPI.ClientEvent.TriggerClientEvent(player, "ExperienceUpdated", player.CurrentExperience, player.CurrentExperience, player.RequiredExperience);

				if (gangId != 0)
				{
					foreach (Weapon weapon in Util.WeaponsByGangId[gangId])
					{
						player.CustomGiveWeapon((WeaponHash)weapon.Hash, weapon.Amount);
					}
				}
			});

			return setFractionResponse.Name;
		}
	}
}
