using System.ComponentModel.DataAnnotations; 
using System.ComponentModel.DataAnnotations.Schema;  

namespace HBAPI.Models
{
    [Table("Classes")]  
    public class DanceClass
    {
        [Key]
        public int Id { get; set; }

        [Required] [StringLength(255)] public string Name { get; set; } = String.Empty;

        public string Description { get; set; } = String.Empty;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public TimeSpan StartTime { get; set; }

        [Required]
        public TimeSpan EndTime { get; set; }

        [Required]
        [Range(0, 9999.99)]
        public decimal Price { get; set; }

        public bool MonthlyEligible { get; set; }

        [StringLength(45)]
        public string? Instructor { get; set; }

        [StringLength(45)]
        public string? Coupon { get; set; }

        // Navigation properties for relationships
        public ICollection<ClassDay> ClassDays { get; set; } = new List<ClassDay>();
        public ICollection<ClassLevel> ClassLevels { get; set; } = new List<ClassLevel>();

        public ICollection<ClassesSessions> ClassesSessions { get; set; } = new List<ClassesSessions>();
    }
}