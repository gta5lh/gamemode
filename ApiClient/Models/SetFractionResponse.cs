using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class SetFractionResponse
    {
        [JsonProperty("tier_name")]
        public string? TierName { get; set; }

        [JsonProperty("tier_required_experience")]
        public short? TierRequiredExperience { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
