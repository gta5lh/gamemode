using System.Text.Json.Serialization;

namespace Gamemode.ApiClient.Models
{
    public class FractionRank
    {
        [JsonPropertyName("id")]
        public byte Id { get; set; }

        [JsonPropertyName("tier")]
        public byte Tier { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("required_experience")]
        public short RequiredExperience { get; set; }
    }
}
