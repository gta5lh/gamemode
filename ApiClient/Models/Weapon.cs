using GTANetworkAPI;
using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class Weapon
    {
        [JsonProperty("hash")]
        public WeaponHash Hash { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        public Weapon(WeaponHash hash, int amount)
        {
            this.Hash = hash;
            this.Amount = amount;
        }
    }
}
