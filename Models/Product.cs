namespace dotnet_store.Models;

public class Product
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public bool IsActive { get; set; }
    public string? Image { get; set; }
    public bool IsHome { get; set; }
    public string? Description { get; set; }
};