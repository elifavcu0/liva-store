using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class AccountChangePasswordModel
{
    [Required]
    [Display(Name = "Current Password")]
    [DataType(DataType.Password)]
    public string? CurrentPassword { get; set; }

    [Required]
    [Display(Name = "New Password")]
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }

    [Required]
    [Display(Name = "New Password (Repeat)")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Passwords don't match.")] // Password alanı ile karşılaştır
    public string? ConfirmNewPassword { get; set; }
}