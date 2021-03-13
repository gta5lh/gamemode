// <copyright file="User.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Repositories.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("users")]
    public class User
    {
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("email", TypeName = "varchar(254)")]
        public string Email { get; set; }

        [Column("name", TypeName = "varchar(32)")]
        public string Name { get; set; }

        [Column("password", TypeName = "char(60)")]
        public string Password { get; set; }

        [Column("admin_rank_id")]
        [ForeignKey("AdminRank")]
        public byte? AdminRankId { get; set; }

        public AdminRank? AdminRank { get; set; }

        [Column("muted_by_id")]
        [ForeignKey("MutedBy")]
        public long? MutedById { get; set; }

        public User? MutedBy { get; set; }

        [Column("muted_until")]
        public DateTime? MutedUntil { get; set; }

        [Column("muted_at")]
        public DateTime? MutedAt { get; set; }

        [Column("mute_reason")]
        public string? MuteReason { get; set; }

        [Column("banned_by_id")]
        [ForeignKey("BannedBy")]
        public long? BannedById { get; set; }

        public User? BannedBy { get; set; }

        [Column("banned_until")]
        public DateTime? BannedUntil { get; set; }

        [Column("banned_at")]
        public DateTime? BannedAt { get; set; }

        [Column("ban_reason")]
        public string? BanReason { get; set; }

        [Column("weapons")]
        public ICollection<Weapon>? Weapons { get; set; }
    }
}
