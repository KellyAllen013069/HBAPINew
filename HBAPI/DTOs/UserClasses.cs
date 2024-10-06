namespace HBAPI.DTOs;

public class UserClassesDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int ClassesSessionsId { get; set; }
    public string? Coupon { get; set; }
    public DateTime CreatedAt { get; set; }

    // New properties for class details
    public string ClassName { get; set; } = string.Empty;
    public string Day { get; set; } = string.Empty;
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }  
    public int Month { get; set; }
}