using System.ComponentModel.DataAnnotations;

namespace liva_store.Models;

public class AccountCreateModel
{
    [Required]
    [Display(Name = "Name - Surname")]
    //[RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "The username can only contain letters and numbers.")]
    public string NameSurname { get; set; } = null!;

    [Required]
    [Display(Name = "Email")]
    [EmailAddress]
    public string Email { get; set; } = null!;


    [Required]
    [Display(Name = "Phone Number")]
    [Phone]
    [RegularExpression(@"^[0-9]{9}$")]
    [MaxLength(9)]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Required]
    [Display(Name = "Password Again")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords don't match.")] // Password alanı ile karşılaştır
    public string ConfirmPassword { get; set; } = null!;
}