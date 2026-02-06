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
                Quantity = quantity
            });
        }
        else
        {
            item.Quantity++;
        }
    }
    public double OrderAmount()
    {
        return CartItems.Sum(i => i.Product.Price * i.Quantity);
    }
    public double Tax()
    {
        return OrderAmount() * 0.2; //vergi %20
    }
    public double Total()
    {
        return OrderAmount() + Tax();
    }
    public int Discount(int discountAmount)
    {
        var quantity = 0;
        foreach (var product in CartItems)
        {
            quantity += product.Quantity;
        }
        return quantity * discountAmount;
    }
}

public class CartItem
{
    public int CartItemId { get; set; }
    public int ProductId { get; set; }
    public int CartId { get; set; }
    public int Quantity { get; set; }
    public Product Product { get; set; } = null!;
    public Cart Cart { get; set; } = null!;
}