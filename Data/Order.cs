namespace dotnet_store.Data;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderTime { get; set; }
    public string Username { get; set; } = null!;
    public string NameSurname { get; set; } = null!;
    public string City { get; set; } = null!;
    public string OpenAddress { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? OrderNote { get; set; }
    public double TotalAmount { get; set; }
    public List<OrderItem> OrderItem { get; set; } = null!;
}

public class OrderItem
{
    public int Id { get; set; } // OrderItem'ın PK'sı
    public int OrderId { get; set; } // OrderId'nin PK'sı yani burada FK
    public Order Order { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public double Price { get; set; } // Ürünün sipariş verildiği zamanki fiyatı, sipariş sonrası güncellemelerden etkilenmemesi için
    public int Quantity { get; set; }
}