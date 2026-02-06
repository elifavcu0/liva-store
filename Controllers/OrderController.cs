using dotnet_store.Data;
using dotnet_store.Models;
using dotnet_store.Services;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace dotnet_store.Controllers;

[Authorize]
public class OrderController : Controller
{
    private ICartService _cartService;
    private readonly IConfiguration _config;
    private readonly DataContext _context;
    public OrderController(ICartService cartService, DataContext context, IConfiguration config)
    {
        _cartService = cartService;
        _context = context;
        _config = config;
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
                PhoneNumber = "+905" + model.PhoneNumber,
                City = model.City,
                PostalCode = model.PostalCode,
                OpenAddress = model.OpenAddress,
                OrderNote = model.OrderNote,
                OrderTime = DateTime.Now,
                TotalAmount = cart.Total(),
                Username = username,
                OrderItem = cart.CartItems.Select(cartItem => new Data.OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Product = cartItem.Product,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.Product.Price,
                }).ToList()
            };

            var payment = await ProcessPayment(model, cart);

            if (payment.Status == "success")
            {
                await _context.Orders.AddAsync(order);
                _context.Carts.Remove(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction("Completed", new { orderId = order.Id });
            }
            ModelState.AddModelError("", payment.ErrorMessage);
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

    private async Task<Payment> ProcessPayment(OrderCreateModel model, Cart cart)
    {
        Options options = new Options();
        options.ApiKey = _config["PaymentAPI:ApiKey"];
        options.SecretKey = _config["PaymentAPI:SecretKey"];
        options.BaseUrl = "https://sandbox-api.iyzipay.com";

        CreatePaymentRequest request = new CreatePaymentRequest();
        request.Locale = Locale.TR.ToString();
        request.ConversationId = Guid.NewGuid().ToString();
        request.Price = (cart.Total() - cart.Discount(1000)).ToString(System.Globalization.CultureInfo.InvariantCulture);
        request.PaidPrice = (cart.Total() - cart.Discount(1000)).ToString(System.Globalization.CultureInfo.InvariantCulture);
        request.Currency = Currency.TRY.ToString();
        request.Installment = 1;
        request.BasketId = "B67832";
        request.PaymentChannel = PaymentChannel.WEB.ToString();
        request.PaymentGroup = PaymentGroup.PRODUCT.ToString();

        PaymentCard paymentCard = new PaymentCard();
        paymentCard.CardHolderName = model.CartOwner;
        paymentCard.CardNumber = model.CartNumber;
        paymentCard.ExpireMonth = model.CartExpirationMonth;
        paymentCard.ExpireYear = model.CartExpirationYear;
        paymentCard.Cvc = model.CartCVV;
        paymentCard.RegisterCard = 0;
        request.PaymentCard = paymentCard;

        Buyer buyer = new Buyer();
        buyer.Id = cart.CustomerId;
        buyer.Name = model.NameSurname;
        buyer.Surname = "Doe";
        buyer.GsmNumber = model.PhoneNumber;
        buyer.Email = "email@email.com";
        buyer.IdentityNumber = "74300864791";
        buyer.LastLoginDate = "2015-10-05 12:43:35";
        buyer.RegistrationDate = "2013-04-21 15:12:09";
        buyer.RegistrationAddress = "Nidakule Göztepe, Merdivenköy Mah. Bora Sok. No:1";
        buyer.Ip = "85.34.78.112";
        buyer.City = model.City;
        buyer.Country = "Turkey";
        buyer.ZipCode = model.PostalCode;
        request.Buyer = buyer;

        Iyzipay.Model.Address address = new Iyzipay.Model.Address();
        address.ContactName = model.NameSurname;
        address.City = model.City;
        address.Country = "Turkey";
        address.Description = model.OpenAddress;
        address.ZipCode = model.PostalCode;
        request.ShippingAddress = address;
        request.BillingAddress = address;

        List<BasketItem> basketItems = new List<BasketItem>();
        foreach (var item in cart.CartItems)
        {
            BasketItem basketItem = new BasketItem();
            basketItem.Id = item.CartId.ToString();
            basketItem.Name = item.Product.Name;
            basketItem.Category1 = item.Product.Category.Name ?? "General";
            basketItem.ItemType = BasketItemType.PHYSICAL.ToString();
            basketItem.Price = (item.Product.Price + item.Product.Price * 0.2 - 1000).ToString(System.Globalization.CultureInfo.InvariantCulture);

            basketItems.Add(basketItem);
        }

        request.BasketItems = basketItems;
        return await Payment.Create(request, options);
    }
}