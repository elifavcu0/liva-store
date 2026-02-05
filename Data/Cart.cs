namespace dotnet_store.Models;

public class Cart
{
    public int CartId { get; set; }
    public string CustomerId { get; set; } = null!;
    public List<CartItem> CartItems { get; set; } = new();

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
    public int TotalProductQuantity()
    {
        var quantity = 0;
        foreach (var product in CartItems)
        {
            quantity += product.Quantity;
        }
        return quantity;
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