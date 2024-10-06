using System.ComponentModel.DataAnnotations;

namespace HBAPI.Models;

public class Translations
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Key_Name { get; set; } 

    [Required]
    public string English_Text { get; set; } 

    [Required]
    public string Spanish_Text { get; set; }  
}