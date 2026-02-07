using System.ComponentModel.DataAnnotations;

namespace liva_store.Models;

public class CategoryEditModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(30)]
    [Display(Name = "Category Name")]
    public string Name { get; set; } = null!;


    [Required]
    [StringLength(30)]
    [Display(Name = "URL")]
    public string Url { get; set; } = null!;
}