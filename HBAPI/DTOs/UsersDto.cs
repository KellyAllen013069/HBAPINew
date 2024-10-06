namespace HBAPI.DTOs;

public class UsersDto
{
    public int Id { get; set; }
    
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string PreferredName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Role { get; set; }
}