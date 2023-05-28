using GeekShopping.OrderAPI.Model.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace GeekShopping.OrderAPI.Model;

[Table("order_header")]
public class OrderHeader : BaseEntity
{
    [Column("user_id")]
    public string UserId { get; set; } = null!;
    [Column("coupon_code")]
    public string? CouponCode { get; set; }
    [Column("purchase_amount")]
    public decimal PurchaseAmount { get; set; }
    [Column("discount_amount")]
    public decimal DiscountAmount { get; set; }
    [Column("first_name")]
    public string FirstName  { get; set; } = null!;
    [Column("last_name")]
    public string LastName  { get; set; } = null!;
    [Column("purchase_date")]
    public DateTime PurchaseDate  { get; set; }
    [Column("order_time")]
    public DateTime OrderTime  { get; set; }
    [Column("phone")]
    public string Phone  { get; set; } = null!;
    [Column("email")]
    public string Email  { get; set; } = null!;
    [Column("card_number")]
    public string CardNumber  { get; set; } = null!;
    [Column("cvv")]
    public string CVV  { get; set; } = null!;
    [Column("expire_month_year")]
    public string ExpireMonthYear  { get; set; } = null!;
    [Column("total_itens")]
    public int CartTotalItens { get; set; }
    public List<OrderDetail>? OrderDetails { get; set; }
    [Column("payment_status")]
    public bool PaymentStatus { get; set; }
}