namespace dotnet_store.Models;

// DB tablo yapısını oluşturduğu için bu bir "entity"dir.
public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Url { get; set; } = null!;
    public List<Product> Products { get; set; } = new(); // Navigation Property
}