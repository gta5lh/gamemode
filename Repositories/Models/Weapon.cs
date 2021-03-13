// <copyright file="Weapon.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

namespace Gamemode.Repositories.Models
{
    using System.ComponentModel.DataAnnotations.Schema;
    using GTANetworkAPI;

    [Table("weapon")]
    public class Weapon
    {
        public Weapon()
        {

        }
        public Weapon(WeaponHash hash, int amount, long userId)
        {
            this.Hash = hash;
            this.Amount = amount;
            this.UserId = userId;
        }

        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Column("hash")]
        public WeaponHash Hash { get; set; }

        [Column("amount")]
        public int Amount { get; set; }

        [Column("user_id")]
        public long UserId { get; set; }

        [Column("user")]
        public User User { get; set; }
    }
}
