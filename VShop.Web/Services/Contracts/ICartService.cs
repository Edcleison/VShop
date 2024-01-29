using VShop.Web.Models;

namespace VShop.Web.Services.Contracts;

public interface ICartService
{
    Task<CartViewModel> GetCartByUserIdAsync(string userId, string token);
    Task<CartViewModel> AddItemToCartAsync(CartViewModel cartViewModel,string token);
    Task<CartViewModel> UpdateCartAsync(CartViewModel cartViewModel,string token);

    Task<bool> RemoveItemFromCartAsync(int cartId, string token);

    //implementação futura
    Task<bool> ApplyCouponAsync(CartViewModel catVM, string token);
    Task<bool> RemoveCouponAsync(string userId, string token);
    Task<bool> ClearCartAsync(string userId, string token);

    Task<CartHeaderViewModel> CheckoutAsync(CartHeaderViewModel cartHeader, string token);
}
