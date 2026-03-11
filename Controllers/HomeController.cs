using Microsoft.AspNetCore.Mvc;
using liva_store.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace liva_store.Controllers;

public class HomeController : Controller
{
    private readonly DataContext _context;
    public HomeController(DataContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var products = await _context.Products.Where(p => p.IsActive && p.IsHome).OrderByDescending(i => i.DiscountRate).ToListAsync();
        ViewData["Categories"] = await _context.Categories.ToListAsync();

        List<int> userWishlistProductsIds = new List<int>();
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var wishlist = await _context.Wishlists.Include(w => w.WishlistItems).FirstOrDefaultAsync(w => w.UserId == userId);
            if (wishlist != null)
            {
                userWishlistProductsIds = wishlist.WishlistItems.Select(wli => wli.ProductId).ToList();
            }
        }
        ViewBag.UserWishlist = userWishlistProductsIds;
        return View(products);
    }
}
