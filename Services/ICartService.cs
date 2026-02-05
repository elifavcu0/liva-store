using dotnet_store.Data;

namespace dotnet_store.Services;

public interface ICartService
{
    string GetCustomerId();
    Task<Cart> GetCart(string customerId);
    Task AddToCart(int productId, int quantity = 1);
    Task RemoveItem(int productId);
    Task TransferCartToUser(string username);
}