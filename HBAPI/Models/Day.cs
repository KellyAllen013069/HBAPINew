using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBAPI.Models
{
    [Table("Days")]
    public class Day
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string DayName { get; set; } = String.Empty;

        // Navigation property to ClassDays
        public ICollection<ClassDay>? ClassDays { get; set; } = new List<ClassDay>();

        public ICollection<ClassesSessions> ClassesSessions { get; set; } = new List<ClassesSessions>();
    }
}