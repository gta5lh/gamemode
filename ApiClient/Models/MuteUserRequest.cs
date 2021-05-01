using System;
using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class MuteUserRequest
    {
        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("muted_by")]
        public long MutedBy { get; set; }

        [JsonProperty("muted_at")]
        public DateTime MutedAt { get; set; }

        [JsonProperty("muted_until")]
        public DateTime MutedUntil { get; set; }

        public MuteUserRequest(string reason, long mutedBy, DateTime mutedAt, DateTime mutedUntil)
        {
            this.Reason = reason;
            this.MutedBy = mutedBy;
            this.MutedAt = mutedAt;
            this.MutedUntil = mutedUntil;
        }
    }
}
