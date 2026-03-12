using System.Security.Claims;
using AspNetCoreGeneratedDocument;
using liva_store.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace liva_store.ViewComponents;

public class WishlistBadgeViewComponent : ViewComponent
{
    private readonly DataContext _context;

    public WishlistBadgeViewComponent(DataContext context)
    {
        _context = context;
    }

    // Bu metot komponent her çağrıldığında otomatik çalışır.
    public async Task<IViewComponentResult> InvokeAsync()
    {
        int wishlistCount = 0;
        if (UserClaimsPrincipal != null && UserClaimsPrincipal.Identity != null && UserClaimsPrincipal.Identity.IsAuthenticated)
        {
            var userId = Convert.ToInt32(UserClaimsPrincipal.FindFirstValue(ClaimTypes.NameIdentifier));
            wishlistCount = await _context.WishlistItems.CountAsync(w => w.Wishlist.UserId == userId);
        }
        return View(wishlistCount);
    }
}