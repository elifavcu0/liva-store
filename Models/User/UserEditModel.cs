using System.ComponentModel.DataAnnotations;
using dotnet_store.Models;

namespace dotnet_store.models;

public class UserEditModel
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Name - Surname")]
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


    [Display(Name = "Temporary Password")]
    [DataType(DataType.Password)]
    public string? Password { get; set; }


    [Display(Name = "User's Role")]
    public IList<string>? SelectedRoles { get; set; }
    public List<AppRole>? Roles { get; set; }
}