using System;

namespace Gamemode.Mechanics.Admin.Models
{
	public enum AdminRank : ushort
	{
		Junior = 1,
		Middle,
		Senior,
		Lead,
		Owner
	}

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
