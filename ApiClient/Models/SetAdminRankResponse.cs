using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class SetAdminRankResponse
    {
        [JsonProperty("rank_after")]
        public short? RankAfter { get; set; }

        [JsonProperty("rank_before")]
        public short? RankBefore { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
