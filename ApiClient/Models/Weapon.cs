using System.Text.Json.Serialization;
using GTANetworkAPI;

namespace Gamemode.ApiClient.Models
{
    public class Weapon
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("hash")]
        public WeaponHash Hash { get; set; }

        [JsonPropertyName("amount")]
        public int Amount { get; set; }
    }
}
