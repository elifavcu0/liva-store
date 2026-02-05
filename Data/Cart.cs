namespace dotnet_store.Models;

public class Cart
{
    public int CartId { get; set; }
    public string CustomerId { get; set; } = null!;
    public List<CartItem> CartItems { get; set; } = new();
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