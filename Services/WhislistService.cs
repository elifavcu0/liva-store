using liva_store.Data;
using Microsoft.EntityFrameworkCore;

namespace liva_store.Services;

public class WhislistService : IWhislistService
{
    private readonly DataContext _context;

    public WhislistService(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> AddItemAsync(int productId, int userId)
    {
        var wishlist = await _context.Wishlists
                                        .Include(w => w.WishlistItems)
                                        .FirstOrDefaultAsync(w => w.UserId == userId);

        if (wishlist == null)
        {
            wishlist = new Wishlist { UserId = userId };
            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();
        }
        bool isItemAlreadyInList = wishlist.WishlistItems.Any(wli => wli.ProductId == productId);

        if (!isItemAlreadyInList)
        {
            var newItem = new WishlistItem
            {
                ProductId = productId,
                WishlistId = wishlist.Id
            };

            _context.WishlistItems.Add(newItem);
            await _context.SaveChangesAsync();

            return true;
        }
        return false;
    }

    public async Task<bool> RemoveItemAsync(int productId, int userId)
    {
        var itemToRemove = await _context.WishlistItems.FirstOrDefaultAsync(wli => wli.ProductId == productId && wli.Wishlist.UserId == userId);

        if (itemToRemove != null)
        {
                _context.WishlistItems.Remove(itemToRemove);
                await _context.SaveChangesAsync();

                return true;
        }
        return false;
    }

    public Task<bool> ToggleWhislistItemAsync(string userId, int productId)
    {
        throw new NotImplementedException();
    }
}