namespace GeekShopping.CouponAPI.Data.ValueObjects;
public class CouponVO
{
    public long Id { get; set; }
    public string CouponCode { get; set; } = null!;
    public decimal DiscountAmount { get; set; }
}