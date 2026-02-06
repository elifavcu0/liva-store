using dotnet_store.Models;

namespace dotnet_store.Data;

public class Cart
{
    public int CartId { get; set; }
    public string CustomerId { get; set; } = null!;
    public List<CartItem> CartItems { get; set; } = new();

    public void AddItem(Product product, int quantity)
    {
        var item = CartItems.FirstOrDefault(i => i.ProductId == product.Id);
        if (item == null)
        {
            CartItems.Add(new CartItem
            {
                Product = product,
                ProductId = product.Id,
                Quantity = quantity
            });
        }
        else
        {
            item.Quantity++;
        }
    }

    public decimal SubTotal => CartItems.Sum(i => i.UnitPrice * i.Quantity);
    public decimal CargoFee
    {
        get
        {
            if (CartItems.Count == 0) return 0;

            var totalQuantity = CartItems.Sum(i => i.Quantity);
            return totalQuantity >= 5 ? 0 : 50;
        }
    }
    public decimal TaxTotal => SubTotal * 0.20m;
    public decimal GrandTotal => SubTotal + TaxTotal + CargoFee;

    public decimal TotalDiscount => CartItems.Sum(i => i.DiscountAmount * i.Quantity);
}

public class CartItem
{
    public int CartItemId { get; set; }
    public int ProductId { get; set; }
    public int CartId { get; set; }
    public int Quantity { get; set; }
    public Product Product { get; set; } = null!;
    public Cart Cart { get; set; } = null!;
    public decimal UnitPrice
    {
        get
        {
            if (Product.DiscountRate > 0)
            {
                return Product.Price - (Product.Price * (Product.DiscountRate / 100m));
            }
            return Product.Price;
        }
    }

    public decimal DiscountAmount
    {
        get
        {
            if (Product.DiscountRate > 0)
            {
                return Product.Price * (Product.DiscountRate / 100m);
            }
            return 0;
        }
    }
}