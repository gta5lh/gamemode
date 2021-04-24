using System.ComponentModel.DataAnnotations.Schema;

namespace Gamemode.Repositories.Models
{
    [Table("fraction_rank")]
    public class FractionRank
    {
        [Column("id", TypeName = "smallint")]
        public byte Id { get; set; }

        [Column("tier", TypeName = "smallint")]
        public byte Tier { get; set; }

        [Column("name", TypeName = "varchar(20)")]
        public string Name { get; set; }

        [Column("fraction_id")]
        [ForeignKey("Fraction")]
        public byte? FractionId { get; set; }

        public Fraction? Fraction { get; set; }

        [Column("required_experience_to_rank_up", TypeName = "smallint")]
        public short RequiredExperienceToRankUp { get; set; }
    }
}
