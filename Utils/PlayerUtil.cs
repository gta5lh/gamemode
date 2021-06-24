// <copyright file="PlayerUtil.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode
{
	using Gamemode.Models.Player;
	using GTANetworkAPI;

	public static class PlayerUtil
	{
		public static CustomPlayer GetByStaticId(long playerId)
		{
			var dynamicId = IdsCache.DynamicIdByStatic(playerId);
			if (dynamicId == null)
			{
				return null;
			}

			return (CustomPlayer)NAPI.Player.GetPlayerFromHandle(new NetHandle((ushort)dynamicId, EntityType.Player));
		}

		public static CustomPlayer GetById(ushort playerId)
		{
			return (CustomPlayer)NAPI.Player.GetPlayerFromHandle(new NetHandle(playerId, EntityType.Player));
		}
	}
}
