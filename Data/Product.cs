using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Data;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal Price { get; set; } // No discount
    public bool IsActive { get; set; }
    public string? Image { get; set; }
    public bool IsHome { get; set; }
    public string? Description { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; } = null!;

    [Range(0, 100, ErrorMessage = "Discount Rate must be  between 0-100.")]
    public int DiscountRate { get; set; } = 0;
    public bool IsOnSale => IsActive && DiscountRate > 0;
    public decimal DiscountedPrice()
    {
        if (DiscountRate != 0) return Price - Price * (DiscountRate / 100m);

        return Price;
    }
};