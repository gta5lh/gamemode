using System;
using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class BanUserRequest
    {
        [JsonProperty("reason")]
        public string Reason { get; set; }

        [JsonProperty("banned_by")]
        public long BannedBy { get; set; }

        [JsonProperty("banned_at")]
        public DateTime BannedAt { get; set; }

        [JsonProperty("banned_until")]
        public DateTime BannedUntil { get; set; }

        public BanUserRequest(string reason, long bannedBy, DateTime bannedAt, DateTime bannedUntil)
        {
            this.Reason = reason;
            this.BannedBy = bannedBy;
            this.BannedAt = bannedAt;
            this.BannedUntil = bannedUntil;
        }
    }
}
