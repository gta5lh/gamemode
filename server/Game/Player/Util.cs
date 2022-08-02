// <copyright file="Util.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Player
{
	using Gamemode.Game.Player.Models;
	using GTANetworkAPI;

	public static class Util
	{
		public static CPlayer? GetByStaticId(string playerId)
		{
			var dynamicId = IdsCache.DynamicIdByStatic(playerId);
			if (dynamicId == null)
			{
				return null;
			}

			return (CPlayer)NAPI.Player.GetPlayerFromHandle(new NetHandle((ushort)dynamicId, EntityType.Player));
		}

		public static CPlayer GetById(ushort playerId)
		{
			return (CPlayer)NAPI.Player.GetPlayerFromHandle(new NetHandle(playerId, EntityType.Player));
		}
	}
}
