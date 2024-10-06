using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBAPI.Models
{
    [Table("Levels")]
    public class Level
    {
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "varchar(20)")]
        public LevelName LevelName { get; set; }

        // Navigation property
        public ICollection<ClassLevel> ClassLevels { get; set; } = new List<ClassLevel>();
    }
}
