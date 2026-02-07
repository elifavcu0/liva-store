using System.ComponentModel.DataAnnotations;

namespace liva_store.Models;

public class AccountAddressModel
{
    [Display(Name = "Open Address")]
    [DataType(DataType.MultilineText)]
    public string? OpenAddress { get; set; }

    [Display(Name = "District")]
    public string District { get; set; } = null!;

    [Display(Name = "City")]
    public string City { get; set; } = null!;

    [Display(Name = "Address Title")]
    public string AddressTitle { get; set; } = null!;
    public int AppUserId { get; set; }

}