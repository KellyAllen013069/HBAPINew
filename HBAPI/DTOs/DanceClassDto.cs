namespace HBAPI.DTOs;

public class DanceClassDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty; 
    public string Description { get; set; } = string.Empty; 
    public string Instructor { get; set; } = string.Empty; 
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public string Coupon { get; set; } = string.Empty; 
    public bool MonthlyEligible { get; set; }
    public List<ClassDayDto> ClassDays { get; set; } = new List<ClassDayDto>();
    public List<ClassLevelDto> ClassLevels { get; set; } = new List<ClassLevelDto>(); 
}