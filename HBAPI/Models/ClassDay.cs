using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HBAPI.Models
{
    [Table("Classes_Days")]
    public class ClassDay
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } 

        [Required] 
        public int ClassId { get; set; }  

        [JsonIgnore] // Prevent circular reference
        public DanceClass? DanceClass { get; set; } 

        [Required] 
        public int DayId { get; set; }  
        public Day? Day { get; set; }  
    }
}