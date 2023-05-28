namespace GeekShopping.OrderAPI.Messages;

public class ProductVO
{
    public long Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? CategoryName { get; set; }
    public string? ImageURL { get; set; }
}
