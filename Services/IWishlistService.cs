namespace liva_store.Services;

public interface IWishlistService
{
    Task<bool> AddItemAsync(int productId, int userId);
    Task<bool> RemoveItemAsync(int productId, int userId);
    Task<bool> ToggleWishlistItemAsync(int userId, int productId);
}