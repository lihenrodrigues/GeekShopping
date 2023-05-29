using Microsoft.AspNetCore.Mvc;
using GeekShopping.CartAPI.Data.ValueObjects;
using GeekShopping.CartAPI.Repository;
using GeekShopping.CartAPI.Messages;
using GeekShopping.CartAPI.RabbitMQSender;

namespace GeekShopping.CartAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CartController : ControllerBase
{
    private ICartRepository _cartRepository;
    private ICouponRepository _couponRepository;
    private IRabbitMQMessageSender _rabbitMQMessageSender;

    public CartController(ICartRepository cartRepository, ICouponRepository couponRepository, IRabbitMQMessageSender rabbitMQMessageSender)
    {
        _cartRepository = cartRepository ?? throw new 
            ArgumentNullException(nameof(cartRepository));
        _couponRepository = couponRepository ?? throw new 
            ArgumentNullException(nameof(couponRepository));
        _rabbitMQMessageSender = rabbitMQMessageSender ?? throw new 
            ArgumentNullException(nameof(rabbitMQMessageSender));
    }

    [HttpGet("find-cart/{id}")]
    public async Task<ActionResult<CartVO>> FindById(string id)
    {
        var cart = await _cartRepository.FindCartByUserId(id);
        if(cart == null) return NotFound();
        return Ok(cart);
    }

    [HttpPost("add-cart")]
    public async Task<ActionResult<CartVO>> AddCart([FromBody] CartVO vo)
    {
        var cart = await _cartRepository.SaveOrUpdateCart(vo);
        if(cart == null) return NotFound();
        return Ok(cart);
    }

    [HttpPut("update-cart")]
    public async Task<ActionResult<CartVO>> UpdateCart([FromBody] CartVO vo)
    {
        var cart = await _cartRepository.SaveOrUpdateCart(vo);
        if(cart == null) return NotFound();
        return Ok(cart);
    }

    [HttpDelete("remove-cart/{id}")]
    public async Task<ActionResult<CartVO>> RemoveCart(int id)
    {
        var status = await _cartRepository.RemoveFromCart(id);
        if(!status) return BadRequest();
        return Ok(status);
    }

    [HttpPost("apply-coupon")]
    public async Task<ActionResult<CartVO>> ApplyCoupon([FromBody] CartVO vo)
    {
        var status = await _cartRepository.ApplyCoupon(vo.CartHeader.UserId, vo.CartHeader.CouponCode);
        if(!status) return NotFound();
        return Ok(status);
    }

    [HttpDelete("remove-coupon/{userId}")]
    public async Task<ActionResult<CartVO>> RemoveCoupon(string userId)
    {
        var status = await _cartRepository.RemoveCoupon(userId);
        if(!status) return NotFound();
        return Ok(status);
    }

    [HttpPost("checkout")]
    public async Task<ActionResult<CheckoutHeaderVO>> Checkout(CheckoutHeaderVO vo)
    {
        string token = Request.Headers["Authorization"];
        if(vo?.UserId == null) return BadRequest();
        var cart = await _cartRepository.FindCartByUserId(vo.UserId);
        if(cart == null) return NotFound();
        if(!string.IsNullOrEmpty(vo.CouponCode))
        {
            CouponVO coupon = await _couponRepository.GetCoupon(token, vo.CouponCode);
            if(vo.DiscountAmount != coupon.DiscountAmount)
            {
                return StatusCode(412);
            }
        }
        vo.CartDetails = cart.CartDetails;
        vo.Time = DateTime.Now;
        
        _rabbitMQMessageSender.SendMessage(vo, "checkoutqueue");

        return Ok(vo);
    }

}
