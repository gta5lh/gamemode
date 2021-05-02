using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class UnmuteUserRequest
    {
        [JsonProperty("unmuted_by")]
        public long UnmutedBy { get; set; }

        public UnmuteUserRequest(long unmutedBy)
        {
            this.UnmutedBy = unmutedBy;
        }
    }
}
