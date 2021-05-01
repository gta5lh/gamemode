using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class FractionRank
    {
        [JsonProperty("id")]
        public byte Id { get; set; }

        [JsonProperty("tier")]
        public byte Tier { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("required_experience")]
        public short RequiredExperience { get; set; }
    }
}
