using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Gamemode.ApiClient.Models
{
    public class User
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("admin_rank_id")]
        public short? AdminRankId { get; set; }

        [JsonProperty("muted_until")]
        public DateTime? MutedUntil { get; set; }

        [JsonProperty("muted_by_id")]
        public long? MutedById { get; set; }

        [JsonProperty("mute_reason")]
        public string? MuteReason { get; set; }

        [JsonProperty("banned_until")]
        public DateTime? BannedUntil { get; set; }

        [JsonProperty("banned_by_id")]
        public long? BannedById { get; set; }

        [JsonProperty("ban_reason")]
        public string? BanReason { get; set; }

        [JsonProperty("fraction_id")]
        public byte? FractionId { get; set; }

        [JsonProperty("fraction_rank")]
        public FractionRank? FractionRank { get; set; }

        [JsonProperty("experience")]
        public short Experience { get; set; }

        [JsonProperty("weapons")]
        public ICollection<Weapon>? Weapons { get; set; }
    }
}
