using System.ComponentModel.DataAnnotations;

namespace liva_store.Models;

public class SliderModel
{
    [Required]
    [Display(Name = "Slider Title")]
    [MaxLength(30, ErrorMessage = "{0} must be maximum 30 characters.")]
    public string? Title { get; set; }

    [Display(Name = "Slider Description")]
    public string? Description { get; set; }

    [Display(Name = "Slider Image")]
    public IFormFile? ImageFile { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; }
    public int Index { get; set; }
}