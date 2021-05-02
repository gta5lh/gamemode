using Gamemode.Models.Admin;
using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class SetAdminRankRequest
    {
        [JsonProperty("rank")]
        public AdminRank Rank { get; set; }

        [JsonProperty("set_by")]
        public long SetBy { get; set; }

        public SetAdminRankRequest(AdminRank rank, long setBy)
        {
            this.Rank = rank;
            this.SetBy = setBy;
        }
    }
}
