using System.ComponentModel.DataAnnotations;

namespace dotnet_store.Models;

public class Address
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Address Title")]
    public string Title { get; set; } = null!;

    [Required]
    [Display(Name = "City")]
    public string City { get; set; } = null!;

    [Required]
    [Display(Name = "District")]
    public string District { get; set; } = null!;

    [Required]
    [Display(Name = "Open Address")]
    [MinLength(25), MaxLength(100)]
    public string OpenAddress { get; set; } = null!;

    // İLİŞKİ KURMA (Foreign Key)
    // Hangi kullanıcıya ait olduğunu bilmemiz lazım
    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;
}