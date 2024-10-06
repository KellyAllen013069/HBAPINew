using System.ComponentModel.DataAnnotations.Schema;

namespace HBAPI.Models;

[Table("Classes_Sessions")]
public class ClassesSessions
{
    public int Id { get; set; }

    // Foreign key for Class
    public int ClassId { get; set; }
    public DanceClass DanceClass { get; set; } = null!; 

    // Foreign key for Day
    public int DayId { get; set; }
    public Day Day { get; set; } = null!;
    
    public ICollection<UserClasses> UserClasses { get; set; } = new List<UserClasses>();
}