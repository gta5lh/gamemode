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
    }
}
