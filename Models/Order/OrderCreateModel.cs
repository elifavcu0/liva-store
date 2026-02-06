using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class OrderCreateModel
{
    [Required]
    [Display(Name = "Name")]
    public string Name { get; set; } = null!;

    [Required]
    [Display(Name = "Surname")]
    public string Surname { get; set; } = null!;

    [Required]
    [Display(Name = "Email")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [Display(Name = "City")]
    public string City { get; set; } = null!;

    [Required]
    [Display(Name = "Open Address")]
    public string OpenAddress { get; set; } = null!;

    [Display(Name = "Postal Code")]
    public string PostalCode { get; set; } = null!;

    [Required]
    [Display(Name = "Phone Number")]
    [Phone]
    [RegularExpression(@"^[0-9]{9}$")]
    [MaxLength(9)]
    public string PhoneNumber { get; set; } = null!;

    [Display(Name = "Order Note")]
    public string? OrderNote { get; set; }

    [Required]
    [Display(Name = "Cart Owner")]
    public string CartOwner { get; set; } = null!;

    [Required]
    [Display(Name = "Cart Number")]
    public string CartNumber { get; set; } = null!;

    [Required]
    [Display(Name = "Cart Expiration Year")]
    public string CartExpirationYear { get; set; } = null!;

    [Required]
    [Display(Name = "Cart Expiration Month")]
    public string CartExpirationMonth { get; set; } = null!;

    [Required]
    [Display(Name = "Cart CVV")]
    public string CartCVV { get; set; } = null!;
}