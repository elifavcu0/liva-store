namespace dotnet_store.Models;

public class OrderCreateModel
{
    public string NameSurname { get; set; } = null!;
    public string City { get; set; } = null!;
    public string OpenAddress { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? OrderNote { get; set; }
}