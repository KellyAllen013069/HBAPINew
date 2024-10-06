namespace HBAPI.DTOs;

public class CartItemDto
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public int ClassId { get; set; }
    public string ClassName { get; set; }
    public string Day { get; set; }
    public decimal Price { get; set; }
    public string CouponCode { get; set; }
    public List<DateTime> Dates { get; set; }
}