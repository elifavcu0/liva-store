using dotnet_store.Data;
using dotnet_store.Models;
using dotnet_store.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_store.Controllers;

[Authorize]
public class OrderController : Controller
{
    private ICartService _cartService;
    private readonly DataContext _context;
    public OrderController(ICartService cartService, DataContext context)
    {
        _cartService = cartService;
        _context = context;
    }

    public async Task<IActionResult> ConfirmCart()
    {
        ViewBag.Cart = await _cartService.GetCart(User.Identity?.Name!);
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ConfirmCart(OrderCreateModel model)
    {
        var username = User.Identity?.Name!;
        var cart = await _cartService.GetCart(username);

        if (cart.CartItems.Count() == 0)
        {
            ModelState.AddModelError("", "Your cart is empty.");
        }

        if (ModelState.IsValid)
        {
            var order = new Order
            {
                NameSurname = model.NameSurname,
                PhoneNumber = model.PhoneNumber,
                City = model.City,
                PostalCode = model.PostalCode,
                OpenAddress = model.OpenAddress,
                OrderNote = model.OrderNote,
                OrderTime = DateTime.Now,
                TotalAmount = cart.Total(),
                Username = username,
                OrderItem = cart.CartItems.Select(cartItem => new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Product = cartItem.Product,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Product.Price,
                }).ToList()
            };

            await _context.Orders.AddAsync(order);
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();

            return RedirectToAction("Completed", new { orderId = order.Id });
        }

        ViewBag.Cart = cart;
        return View(model);
    }

    public IActionResult Completed(string orderId)
    {
        return View("Completed",orderId);
    }
}