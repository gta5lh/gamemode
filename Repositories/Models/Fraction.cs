namespace Gamemode.Repositories.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("fraction")]
    public class Fraction
    {
        [Column("id", TypeName = "smallint")]
        public byte Id { get; set; }

        [Column("name", TypeName = "varchar(20)")]
        public string Name { get; set; }
    }
}
