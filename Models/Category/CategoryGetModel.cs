namespace dotnet_store.Models;

// DB'den veri taşıdığı için bu bir "Model"dir.
public class CategoryGetModel
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string Url { get; set; } = null!;
    public int ProductQuantity { get; set; }
}