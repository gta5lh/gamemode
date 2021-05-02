using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class UnbanUserRequest
    {
        [JsonProperty("unbanned_by")]
        public long UnbannedBy { get; set; }

        public UnbanUserRequest(long unbannedBy)
        {
            this.UnbannedBy = unbannedBy;
        }
    }
}
