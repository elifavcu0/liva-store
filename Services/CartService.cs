using dotnet_store.Data;
using Microsoft.EntityFrameworkCore;

namespace dotnet_store.Services;

public class CartService : ICartService
{
    private readonly DataContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CartService(DataContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCustomerId()
    {
        var context = _httpContextAccessor.HttpContext;
        return context?.User.Identity?.Name ?? context?.Request.Cookies["customerId"]!;
    }
    public async Task AddToCart(int productId, int quantity = 1)
    {
        var cart = await GetCart(GetCustomerId());
        var item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);
        var product = await _context.Products.FirstOrDefaultAsync(i => i.Id == productId);

        if (product != null)
        {
            cart.AddItem(product, quantity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<Cart> GetCart(string custId)
    {
        var cart = await _context.Carts.Include(i => i.CartItems).ThenInclude(i => i.Product).Where(i => i.CustomerId == custId).FirstOrDefaultAsync();

        if (cart == null)
        {
            var customerId = _httpContextAccessor.HttpContext?.User.Identity?.Name;

            if (string.IsNullOrEmpty(customerId))
            {
                customerId = Guid.NewGuid().ToString();

                var cookieOptions = new CookieOptions
                {
                    Expires = DateTime.Now.AddMonths(1),
                    IsEssential = true
                };
                _httpContextAccessor.HttpContext?.Response.Cookies.Append("customerId", customerId, cookieOptions);
            }
            cart = new Cart { CustomerId = customerId };
            await _context.Carts.AddAsync(cart); // change tracking
            await _context.SaveChangesAsync();
        }
        return cart;
    }

    public async Task RemoveItem(int productId)
    {
        var cart = await GetCart(GetCustomerId());
        var item = cart.CartItems.FirstOrDefault(i => i.ProductId == productId);

        if (item != null)
        {
            cart.CartItems.Remove(item);
            await _context.SaveChangesAsync();
        }
    }

    public async Task TransferCartToUser(string username)
    {
        var cookieId = _httpContextAccessor.HttpContext?.Request.Cookies["customerId"];
        if (string.IsNullOrEmpty(cookieId)) return;

        var cookieCart = await GetCart(_httpContextAccessor.HttpContext?.Request.Cookies["customerId"]!);
        var userCart = await GetCart(username);

        foreach (var item in cookieCart.CartItems)
        {
            var cartItem = userCart!.CartItems.FirstOrDefault(i => i.ProductId == item.ProductId);
            if (cartItem != null)
            {
                cartItem.Quantity += item.Quantity;
            }
            else
            {
                userCart?.CartItems.Add(new CartItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    CartId = userCart.CartId
                });
            }
        }
        _context.Carts.Remove(cookieCart);
        await _context.SaveChangesAsync();
        _httpContextAccessor.HttpContext?.Response.Cookies.Delete("customerId");
    }
}