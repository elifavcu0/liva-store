namespace liva_store.Models;

public class ProductGetModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; }
    public bool IsActive { get; set; }
    public string? Image { get; set; }
    public bool IsHome { get; set; }
    public string Category { get; set; } = null!;
    public int DiscountRate { get; set; } = 0;
};