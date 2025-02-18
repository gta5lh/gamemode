// <copyright file="AdminRankMethods.cs" company="Lost Heaven">
// Copyright (c) Lost Heaven. All rights reserved.
// </copyright>

namespace Gamemode.Game.Admin.Models
{
	using System;

	public static class AdminRankMethods
	{
		public static bool AtLeast(this AdminRank adminRank, AdminRank atLeast)
		{
			return adminRank >= atLeast;
		}

		public static bool IsAdmin(this AdminRank adminRank)
		{
			return adminRank >= AdminRank.Junior;
		}

		public static string ToPosition(this AdminRank adminRank)
		{
			switch (adminRank)
			{
				case AdminRank.Junior:
					return "начинающий";

				case AdminRank.Middle:
					return "продвинутый";

				case AdminRank.Senior:
					return "старший";

				case AdminRank.Lead:
					return "руководствующий";

				case AdminRank.Owner:
					return "владелец";

				default:
					return "игрок";
			}
		}

		public static AdminRank ToAdminRank(this string enumString)
		{
			AdminRank adminRank = (AdminRank)Enum.Parse(typeof(AdminRank), enumString);
			if (adminRank < 0 || adminRank > AdminRank.Owner)
			{
				throw new ArgumentOutOfRangeException();
			}

			return adminRank;
		}
	}
}
