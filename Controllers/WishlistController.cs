using System.Security.Claims;
using liva_store.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace liva_store.Controllers;

[Authorize]
public class WishlistController : Controller
{
    private readonly IWishlistService _wishlistService;

    public WishlistController(IWishlistService wishlistService)
    {
        _wishlistService = wishlistService;
    }

    [HttpPost]
    public async Task<IActionResult> Toggle(int productId)
    {
        var userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
        bool isAdded = await _wishlistService.ToggleWishlistItemAsync(userId, productId);

        return Json(new { success = true, isAdded = isAdded }); // istenilen işlemin sonucu arka planda sunucuya gönderiliyor
    }
}