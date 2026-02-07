using System.ComponentModel.DataAnnotations;

namespace liva_store.Models;

public class AccountChangePasswordModel
{
    [Required]
    [Display(Name = "Current Password")]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; } = null!;

    [Required]
    [Display(Name = "New Password")]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = null!;

    [Required]
    [Display(Name = "New Password (Repeat)")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Passwords don't match.")]
    public string ConfirmNewPassword { get; set; } = null!;
}