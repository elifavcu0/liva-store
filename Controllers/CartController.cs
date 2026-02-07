using liva_store.Models;
using liva_store.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using liva_store.Services;

namespace liva_store.Controllers;

public class CartController : Controller
{
    private readonly ICartService _cartService;
    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    public async Task<IActionResult> Index()
    {
        var customerId = _cartService.GetCustomerId();
        var cart = await _cartService.GetCart(customerId);
        return View(cart);
    }

    [HttpPost]
    public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
    {
        await _cartService.AddToCart(productId, quantity);

        return RedirectToAction("Index", "Cart");
    }

    [HttpPost]
    public async Task<IActionResult> RemoveFromCart(int productId)
    {
        await _cartService.RemoveItem(productId);

        return RedirectToAction("Index", "Cart");
    }
}