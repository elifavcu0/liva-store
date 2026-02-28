using liva_store.Data;
using Microsoft.EntityFrameworkCore;

namespace liva_store.Services;

public class WishlistService : IWishlistService
{
    private readonly DataContext _context;

    public WishlistService(DataContext context)
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

    public async Task<bool> ToggleWishlistItemAsync(int productId, int userId)
    {
        var wishlist = await _context.Wishlists.Include(wli => wli.WishlistItems).FirstOrDefaultAsync(w => w.UserId == userId);

        if (wishlist == null)
        {
            wishlist = new Wishlist { UserId = userId };
            _context.Wishlists.Add(wishlist);
            await _context.SaveChangesAsync();
        }
        var existingItem = wishlist.WishlistItems.FirstOrDefault(wli => wli.ProductId == productId);

        if (existingItem != null)
        {
            _context.WishlistItems.Remove(existingItem);
            await _context.SaveChangesAsync();

            return false;
        }
        else
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
    }
}