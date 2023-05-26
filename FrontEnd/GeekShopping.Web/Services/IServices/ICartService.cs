namespace GeekShopping.Web.Services.IServices;
using GeekShopping.Web.Models;
public interface ICartService
{
    Task<CartViewModel> FindCartByUserId(string token, string userId);
    Task<CartViewModel> AddItemToCart(string token, CartViewModel cart);
    Task<CartViewModel> UpdateCart(string token, CartViewModel cart);
    Task<bool> RemoveFromCart(string token, long cartId);

    Task<bool> ApplyCoupon(string token, CartViewModel cart);
    Task<bool> RemoveCoupon(string token, string userId);
    Task<bool> ClearCart(string token, string userId);

    Task<CartViewModel> Checkout(string token, CartHeaderViewModel cartHeader);
}
