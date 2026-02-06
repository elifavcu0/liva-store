using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class ProductModel()
{
    [Required(ErrorMessage = "{0} field cannot be empty.")]
    [Display(Name = "Product Name")]
    public string Name { get; set; } = null!;

    [Required(ErrorMessage = "{0} field cannot be empty.")]
    [Range(5, 350000, ErrorMessage = "{0} must be between {2} - {1}")]
    [Display(Name = "Price")]
    public decimal? Price { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; }
    public IFormFile? ImageFile { get; set; }

    [Display(Name = "Home")]
    public bool IsHome { get; set; }

    [Display(Name = "Description")]
    [Required(ErrorMessage = "{0} field cannot be empty.")]
    public string? Description { get; set; }

    [Display(Name = "Category")]
    [Required(ErrorMessage = "{0} field cannot be empty.")]
    public int? CategoryId { get; set; }
    public int DiscountRate { get; set; } = 0;
}