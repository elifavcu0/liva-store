namespace liva_store.Data;

public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = null!;
    public DateTime OrderDate { get; set; }
    public string Username { get; set; } = null!;
    public string Name{ get; set; } = null!;
    public string Surname { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string City { get; set; } = null!;
    public string OpenAddress { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? OrderNote { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal TotalDiscount { get; set; }
    public decimal CargoFee { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new();
}

public class OrderItem
{
    public int Id { get; set; } // OrderItem'ın PK'sı
    public int OrderId { get; set; } // OrderId'nin PK'sı yani burada FK
    public Order Order { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public decimal Price { get; set; } // Ürünün sipariş verildiği zamanki fiyatı, sipariş sonrası güncellemelerden etkilenmemesi için
    public decimal OriginalPrice { get; set; }
    public int Quantity { get; set; }
}