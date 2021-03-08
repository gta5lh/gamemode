namespace Gamemode.Models.Admin
{
    public enum AdminRank : ushort
    {
        Junior = 1,
        Middle,
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
    }
}
