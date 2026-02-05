using dotnet_store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_store.Controllers;

public class CartController : Controller
{
    private readonly DataContext _context;
    public CartController(DataContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var cart = await GetCart();
        return View(cart);
    }

    private async Task<Cart> GetCart()
    {
        var customerId = User.Identity?.Name ?? Request.Cookies["customerId"]; // eğer kullanıcı kayıtlı değilse customerId isimli benzersiz bir cookie oluştur.

        var cart = await _context.Carts.Include(i => i.CartItems).ThenInclude(i => i.Product).Where(i => i.CustomerId == customerId).FirstOrDefaultAsync();

        if (cart == null)
        {
            customerId = User.Identity?.Name;

            if (string.IsNullOrEmpty(customerId))
            {
                customerId = Guid.NewGuid().ToString();

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddMonths(1),
                    IsEssential = true
                };
                Response.Cookies.Append("customerId", customerId, cookieOptions);
            }

            cart = new Cart { CustomerId = customerId};
            await _context.Carts.AddAsync(cart); // change tracking
        }
        return cart;
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
    {
        var cart = await GetCart();
        var item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);

        if (item != null)
        {
            // ürün önceden eklenmiş
            item!.Quantity++;
        }
        else
        {
            //ürün ilk defa ekleniyor
            cart.CartItems.Add(new CartItem
            {
                ProductId = productId,
                Quantity = quantity
            });
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Cart");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int productId)
    {
        var cart = await GetCart();
        var item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);

        if (item != null)
        {
            cart.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }
        else
        {
            TempData["Error"] = "There's no such a product.";
        }
        return RedirectToAction("Index", "Cart");
    }
}