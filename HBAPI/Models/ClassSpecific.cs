using System.ComponentModel.DataAnnotations.Schema;

namespace HBAPI.Models;

[Table("Classes_Specifics")]
public class ClassSpecific
{
    public int Id { get; set; }  
    public int ClassId { get; set; }
    public DanceClass DanceClass { get; set; } = new DanceClass();
    public DateTime Date { get; set; }
}
