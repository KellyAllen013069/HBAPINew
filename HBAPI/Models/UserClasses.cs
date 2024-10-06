using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HBAPI.Models;

[Table("User_Classes")]
public class UserClasses
{
    public int Id { get; set; }
    public string UserId { get; set; }
        
    [Column("Classes_SessionsId")]
    public int ClassesSessionsId { get; set; }
    public ClassesSessions ClassesSessions { get; set; } = null!;

    public string? Coupon { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    
    
}