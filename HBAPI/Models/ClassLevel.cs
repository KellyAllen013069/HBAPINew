using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HBAPI.Models;

[Table("Classes_Levels")]
public class ClassLevel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    public int ClassId { get; set; }

    [JsonIgnore] // Prevent circular reference
    public DanceClass? DanceClass { get; set; }
    
    public int LevelId { get; set; }
    public Level? Level { get; set; }
}