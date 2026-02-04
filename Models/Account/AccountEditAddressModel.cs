using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class AccountEditAddressModel
{
    [Display(Name = "Address")]
    [DataType(DataType.MultilineText)]
    public string? Address { get; set; }

    [Display(Name = "Distinct")]
    public string Distinct { get; set; } = null!;

    [Display(Name = "City")]
    public string City { get; set; } = null!;

    [Display(Name = "Address Title")]
    public string AddressTitle { get; set; } = null!;
}