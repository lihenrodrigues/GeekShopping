using GeekShopping.CouponAPI.Repository;
using GeekShopping.CouponAPI.Data.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GeekShopping.CouponAPI.Controllers;
[Route("api/v1/[controller]")]
public class CouponController : Controller
{
    private ICouponRepository _repository;

    public CouponController(ICouponRepository repository)
    {
        _repository = repository ?? throw new 
            ArgumentNullException(nameof(repository));
    }

    [Authorize]
    [HttpGet("{couponCode}")]
    public async Task<ActionResult<CouponVO>> GetCouponByCouponCode(string couponCode)
    {
        var coupon = await _repository.GetCouponByCouponCode(couponCode);
        if(coupon == null) return NotFound();
        return Ok(coupon);
    }
}