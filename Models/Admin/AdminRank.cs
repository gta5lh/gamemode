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
        public static bool AtLeastJunior(this AdminRank adminRank)
        {
            return (ushort)adminRank >= 1;
        }

        public static bool AtLeast(this AdminRank adminRank, AdminRank atLeast)
        {
            return adminRank >= atLeast;
        }
    }
}
