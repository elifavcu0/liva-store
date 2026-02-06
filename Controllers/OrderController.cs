using dotnet_store.Data;
using dotnet_store.Models;
using dotnet_store.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

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

    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Index()
    {
        return View(await _context.Orders.ToListAsync());
    }

    [Authorize(Roles = "Admin")]
    public IActionResult Details(int id)
    {
        var order = _context.Orders.Include(i => i.OrderItem).ThenInclude(i => i.Product).FirstOrDefault(i => i.Id == id);

        if (order == null) return NotFound();
        return View(order);
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
        return View("Completed", orderId);
    }

    public async Task<IActionResult> OrderList()
    {
        var username = User.Identity?.Name;
        var orders = await _context.Orders.Include(i => i.OrderItem).ThenInclude(i => i.Product).Where(i => i.Username == username).ToListAsync();
        return View(orders);
    }
}