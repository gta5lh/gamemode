using GTANetworkAPI;
using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class Weapon
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("hash")]
        public WeaponHash Hash { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        public Weapon(long id, WeaponHash hash, int amount)
        {
            this.Id = id;
            this.Hash = hash;
            this.Amount = amount;
        }
    }
}
