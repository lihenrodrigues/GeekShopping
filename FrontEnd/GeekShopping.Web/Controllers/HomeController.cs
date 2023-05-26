﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using GeekShopping.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using GeekShopping.Web.Services.IServices;
using System.Text.Json;

namespace GeekShopping.Web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IProductService _productService;
    private readonly ICartService _cartService;


    public HomeController(ILogger<HomeController> logger,  IProductService productService, ICartService cartService)
    {
        _logger = logger;
        _productService = productService;
        _cartService = cartService;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _productService.FindAllProducts("");
        return View(products);
    }

    [Authorize]
    public async Task<IActionResult> Details(int id)
    {
        var token = await HttpContext.GetTokenAsync("access_token");
        var model = await _productService.FindProductById(token, id);
        return View(model);
    }
    
    [HttpPost]
    [ActionName("Details")] 
    [Authorize]
    public async Task<IActionResult> DetailsPost(ProductViewModel model)
    {
        var token = await HttpContext.GetTokenAsync("access_token");

        CartViewModel cart = new() 
        {
            CartHeader = new CartHeaderViewModel 
            {
                UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value
            }
        };

        CartDetailViewModel cartDetail = new CartDetailViewModel()
        {
            Count = model.Count,
            ProductId = model.Id,
            Product = await _productService.FindProductById(token, model.Id)
        };

        List<CartDetailViewModel> cartDetails = new List<CartDetailViewModel>();
        cartDetails.Add(cartDetail);

        cart.CartDetails = cartDetails;
        Console.WriteLine(JsonSerializer.Serialize(cart));

        var response = await _cartService.AddItemToCart(token, cart);
        if(response != null)
        {
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [Authorize]
    public async Task<IActionResult> Login()
    {
        return RedirectToAction(nameof(Index));
    }

    public IActionResult Logout()
    {
        return SignOut("Cookies", "oidc");
    }

}
