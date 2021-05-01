using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Gamemode.ApiClient.Models
{
    public class User
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("admin_rank_id")]
        public short? AdminRankId { get; set; }

        [JsonPropertyName("muted_until")]
        public DateTime? MutedUntil { get; set; }

        [JsonPropertyName("muted_by_id")]
        public long? MutedById { get; set; }

        [JsonPropertyName("mute_reason")]
        public string? MuteReason { get; set; }

        [JsonPropertyName("fraction_id")]
        public byte? FractionId { get; set; }

        [JsonPropertyName("fraction_rank")]
        public FractionRank? FractionRank { get; set; }

        [JsonPropertyName("experience")]
        public short Experience { get; set; }

        [JsonPropertyName("weapons")]
        public ICollection<Weapon>? Weapons { get; set; }
    }
}
