namespace HBAPI.DTOs
{
    public class ClassesSessionsDto
    {
        public int Id { get; set; }
        public int ClassId { get; set; }
        public string ClassName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Instructor { get; set; } = string.Empty;
        public List<string> Levels { get; set; } = new List<string>(); // Changed to List to handle multiple levels
        public int DayId { get; set; }
        public string DayName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public TimeSpan StartTime { get; set; }  
        public TimeSpan EndTime { get; set; } 
    }
}