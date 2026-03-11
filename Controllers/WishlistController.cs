using System.Security.Claims;
using liva_store.Data;
using liva_store.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace liva_store.Controllers;

[Authorize]
public class WishlistController : Controller
{
    private readonly IWishlistService _wishlistService;
    private readonly DataContext _context;

    public WishlistController(IWishlistService wishlistService, DataContext context)
    {
        _wishlistService = wishlistService;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var wishlistItems = await _context.WishlistItems.Include(wli => wli.Product).Where(w => w.Wishlist.UserId == userId).ToListAsync();
        return View(wishlistItems);
    }
    [HttpPost]
    public async Task<IActionResult> Toggle(int productId)
    {
        var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        bool isAdded = await _wishlistService.ToggleWishlistItemAsync(userId, productId);

        return Json(new { success = true, isAdded = isAdded }); // istenilen işlemin sonucu arka planda sunucuya gönderiliyor
    }
}