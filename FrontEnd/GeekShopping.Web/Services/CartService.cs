using GeekShopping.Web.Models;
using GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Utils;
using System.Net.Http.Headers;
using System.Net.Http;

namespace GeekShopping.Web.Services;
public class CartService : ICartService
{
    private readonly HttpClient _client;
    public const string BasePath = "api/v1/cart";

    public CartService(HttpClient client)
    {
        _client = client ?? throw new ArgumentNullException(nameof(client));
    }


    public async Task<CartViewModel> FindCartByUserId(string token, string userId)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.GetAsync($"{BasePath}/find-cart/{userId}");
        return await response.ReadContentAs<CartViewModel>();
    }

    public async Task<CartViewModel> AddItemToCart(string token, CartViewModel model)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.PostAsJson($"{BasePath}/add-cart", model);
        if(response.IsSuccessStatusCode) 
            return await response.ReadContentAs<CartViewModel>();
        else 
            throw new Exception("Something went wrong when calling API");
    }

    public async Task<CartViewModel> UpdateCart(string token, CartViewModel model)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.PutAsJson($"{BasePath}/update-cart", model);
        if(response.IsSuccessStatusCode) 
            return await response.ReadContentAs<CartViewModel>();
        else 
            throw new Exception("Something went wrong when calling API");
    }

    public async Task<bool> RemoveFromCart(string token, long cartId)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.DeleteAsync($"{BasePath}/remove-cart/{cartId}");
        if(response.IsSuccessStatusCode) 
            return await response.ReadContentAs<bool>();
        else 
            throw new Exception("Something went wrong when calling API");
    }

    public async Task<bool> ApplyCoupon(string token, CartViewModel model)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.PostAsJson($"{BasePath}/apply-coupon", model);
        if(response.IsSuccessStatusCode) 
            return await response.ReadContentAs<bool>();
        else 
            throw new Exception("Something went wrong when calling API");
    }

    public async Task<bool> RemoveCoupon(string token, string userId)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.DeleteAsync($"{BasePath}/remove-coupon/{userId}");
        if(response.IsSuccessStatusCode) 
            return await response.ReadContentAs<bool>();
        else 
            throw new Exception("Something went wrong when calling API");
    }

    public async Task<Object> Checkout(string token, CartHeaderViewModel model)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var response = await _client.PostAsJson($"{BasePath}/checkout", model);
        if(response.IsSuccessStatusCode) 
        {
            return await response.ReadContentAs<CartHeaderViewModel>();
        }
        else if (response.StatusCode.ToString().Equals("PreconditionFailed")) 
        {
            return "Coupon Price has changed, please confirm!";
        }
        else 
            throw new Exception("Something went wrong when calling API");
    }

    public async Task<bool> ClearCart(string token, string userId)
    {
        throw new NotImplementedException();
    }

}
