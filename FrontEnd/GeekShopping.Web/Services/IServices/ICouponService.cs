using GeekShopping.Web.Models;

namespace GeekShopping.Web.Services.IServices;
public interface ICouponService
{
    Task<CouponViewModel> GetCoupon(string token, string code);
}
