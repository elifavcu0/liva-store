namespace liva_store.Services;

public interface IWhislistService
{
    Task<bool> AddItemAsync(int productId, int userId);
    Task<bool> RemoveItemAsync(int productId, int userId);
    Task<bool> ToggleWhislistItemAsync(string userId, int productId);
}