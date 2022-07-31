// <copyright file="VipRankMethods.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Player.Models
{
	using System;

	public static class VipRankMethods
	{
		public static bool AtLeast(this VipRank vipRank, VipRank atLeast)
		{
			return vipRank >= atLeast;
		}

		public static bool IsVip(this VipRank vipRank)
		{
			return vipRank >= VipRank.Basic;
		}

		public static string ToPosition()
		{
			return string.Empty;
		}

		public static VipRank ToVipRank(this string enumString)
		{
			VipRank vipRank = (VipRank)Enum.Parse(typeof(VipRank), enumString);
			if (vipRank < 0 || vipRank > VipRank.Premium)
			{
				throw new ArgumentOutOfRangeException();
			}

			return vipRank;
		}
	}
}
