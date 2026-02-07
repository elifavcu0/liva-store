using System.ComponentModel.DataAnnotations;

namespace liva_store.Models;

public class CategoryCreateModel
{
    [Display(Name = "Category Name")]
    [Required]
    [StringLength(30)]
    public string Name { get; set; } = null!;


    [Display(Name = "URL")]
    [Required]
    [StringLength(30)]
    public string Url { get; set; } = null!;
}