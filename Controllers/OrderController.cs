using System.Security.Cryptography;
using liva_store.Data;
using liva_store.Models;
using liva_store.Services;
using Iyzipay;
using Iyzipay.Model;
using Iyzipay.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace liva_store.Controllers;

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
        var order = _context.Orders.Include(i => i.OrderItems).ThenInclude(i => i.Product).FirstOrDefault(i => i.Id == id);

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
            string cleanPhone = model.PhoneNumber?.Replace(" ", "").Trim() ?? "";
            model.PhoneNumber = "+905" + cleanPhone;

            var order = new Order
            {
                OrderNumber = new Random().Next(100000, 999999).ToString(),
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                City = model.City,
                PostalCode = model.PostalCode,
                OpenAddress = model.OpenAddress,
                OrderNote = model.OrderNote,
                OrderDate = DateTime.Now,
                TotalAmount = cart.GrandTotal,
                Username = username,
                CargoFee = cart.CargoFee,
                TotalDiscount = cart.TotalDiscount,
                TaxAmount = cart.TaxTotal,
                OrderItems = cart.CartItems.Select(cartItem => new Data.OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    Price = cartItem.UnitPrice,
                    OriginalPrice = cartItem.Product.Price
                }).ToList()
            };

            var payment = await ProcessPayment(model, cart,order);

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
        var orders = await _context.Orders.Include(i => i.OrderItems).ThenInclude(i => i.Product).Where(i => i.Username == username).ToListAsync();
        return View(orders);
    }
    private async Task<Payment> ProcessPayment(OrderCreateModel model, Cart cart, Order order)
    {
        Options options = new Options();
        options.ApiKey = _config["PaymentAPI:ApiKey"];
        options.SecretKey = _config["PaymentAPI:SecretKey"];
        options.BaseUrl = "https://sandbox-api.iyzipay.com";

        CreatePaymentRequest request = new CreatePaymentRequest();
        request.Locale = Locale.TR.ToString();
        request.ConversationId = Guid.NewGuid().ToString();
        request.Price = cart.GrandTotal.ToString(System.Globalization.CultureInfo.InvariantCulture);
        request.PaidPrice = cart.GrandTotal.ToString(System.Globalization.CultureInfo.InvariantCulture);
        request.Currency = Currency.TRY.ToString();
        request.Installment = 1;
        request.BasketId = order.OrderNumber;
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
        buyer.Name = model.Name;
        buyer.Surname = model.Surname;
        buyer.GsmNumber = model.PhoneNumber;
        buyer.Email = model.Email;
        buyer.IdentityNumber = "74300864791";
        buyer.LastLoginDate = "2015-10-05 12:43:35";
        buyer.RegistrationDate = "2013-04-21 15:12:09";
        buyer.RegistrationAddress = model.OpenAddress;
        buyer.Ip = "85.34.78.112";
        buyer.City = model.City;
        buyer.Country = "Turkey";
        buyer.ZipCode = model.PostalCode;
        request.Buyer = buyer;

        Iyzipay.Model.Address address = new Iyzipay.Model.Address();
        address.ContactName = model.Name + " " + model.Surname;
        address.City = model.City;
        address.Country = "Turkey";
        address.Description = model.OpenAddress;
        address.ZipCode = model.PostalCode;
        request.ShippingAddress = address;
        request.BillingAddress = address;

        List<BasketItem> basketItems = new List<BasketItem>();
        foreach (var item in cart.CartItems)
        {
            for (int i = 0; i < item.Quantity; i++)
            {
                BasketItem basketItem = new BasketItem();
                basketItem.Id = item.ProductId.ToString();
                basketItem.Name = item.Product.Name;
                basketItem.Category1 = item.Product.Category.Name ?? "General";
                basketItem.ItemType = BasketItemType.PHYSICAL.ToString();

                decimal itemPriceWithTax = item.UnitPrice * 1.20m;

                basketItem.Price = itemPriceWithTax.ToString(System.Globalization.CultureInfo.InvariantCulture);

                basketItems.Add(basketItem);
            }
        }
        if (cart.CargoFee > 0)
        {
            BasketItem shippingItem = new BasketItem
            {
                Id = "Cargo",
                Name = "Cargo Fee",
                Category1 = "Cargo",

                ItemType = BasketItemType.VIRTUAL.ToString(),

                Price = cart.CargoFee.ToString(System.Globalization.CultureInfo.InvariantCulture)
            };

            basketItems.Add(shippingItem);
        }

        request.BasketItems = basketItems;
        return await Payment.Create(request, options);
    }
}