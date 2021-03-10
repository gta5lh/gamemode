using Gamemode.Models.Admin;
using GTANetworkAPI;
using MongoDB.Bson.Serialization.Attributes;

namespace Gamemode.Models.User
{
    public class User
    {
        [BsonId]
        public long Id { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public AdminRank AdminRank { get; set; }

        public MuteState? MuteState { get; set; }

        public Weapon[] Weapons { get; set; }
    }

    public class Weapon
    {
        public WeaponHash WeaponHash { get; set; }

        public int Amount { get; set; }

        public Weapon(WeaponHash weaponHash, int amount)
        {
            this.WeaponHash = weaponHash;
            this.Amount = amount;
        }
    }
}
