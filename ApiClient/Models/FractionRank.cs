using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class FractionRank
    {
        [JsonProperty("tier")]
        public byte? Tier { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("required_experience")]
        public short? RequiredExperience { get; set; }

        [JsonProperty("skin")]
        public long? Skin { get; set; }
    }
}
