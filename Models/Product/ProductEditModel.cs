using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class ProductEditModel
{
    public int Id { get; set; }

    [Display(Name = "Product Name")]
    public string Name { get; set; } = null!;

    [Display(Name = "Price")]
    public double Price { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; }
    public IFormFile? ImageFile { get; set; }
    public string? ImageName { get; set; }

    [Display(Name = "Home")]
    public bool IsHome { get; set; }

    [Display(Name = "Description")]
    public string? Description { get; set; }

    [Display(Name = "Category")]
    public int CategoryId { get; set; }
}