using GTANetworkAPI;
using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class GiveWeaponRequest
    {
        [JsonProperty("weapon_hash")]
        public WeaponHash WeaponHash { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("given_by")]
        public long GivenBy { get; set; }

        public GiveWeaponRequest(WeaponHash weaponHash, int amount, long givenBy)
        {
            WeaponHash = weaponHash;
            Amount = amount;
            GivenBy = givenBy;
        }
    }
}
