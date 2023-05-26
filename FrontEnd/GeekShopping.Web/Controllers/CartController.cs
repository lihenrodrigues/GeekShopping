using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Models;

namespace GeekShopping.Web.Controllers;

public class CartController : Controller
{
    private readonly ILogger<CartController> _logger;
    private readonly IProductService _productService;
    private readonly ICartService _cartService;


    public CartController(ILogger<CartController> logger,  IProductService productService, ICartService cartService)
    {
        _logger = logger;
        _productService = productService;
        _cartService = cartService;
    }

    [Authorize]
    public async Task<IActionResult> CartIndex()
    {
        return View(await FindUserCart());
    }

    public async Task<IActionResult> Remove(int id)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;

        var response = await _cartService.RemoveFromCart(token, id);

        if(response)
        {
            return RedirectToAction(nameof(CartIndex));
        }

        return View();
    }

    private async Task<CartViewModel> FindUserCart(){
        var token = await HttpContext.GetTokenAsync("access_token");
        var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;

        var response = await _cartService.FindCartByUserId(token, userId);

        if(response?.CartHeader != null)
        {
            foreach (var detail in response.CartDetails)
            {
                response.CartHeader.PurchaseAmount += (detail.Product.Price * detail.Count);
            }
        }

        return response;
    }
}