// <copyright file="AdminRank.cs" company="lbyte00">
// Copyright (c) lbyte00. All rights reserved.
// </copyright>

using System.ComponentModel.DataAnnotations.Schema;

namespace Gamemode.Repositories.Models
{
    [Table("admin_rank")]
    public class AdminRank
    {
        [Column("id", TypeName = "smallint")]
        public byte Id { get; set; }

        [Column("name", TypeName = "varchar(20)")]
        public string Name { get; set; }
    }
}
