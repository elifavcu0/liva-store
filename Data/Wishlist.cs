namespace liva_store.Data;

public class Wishlist
{
    public int Id { get; set; }
    public AppUser User { get; set; } = null!;
    public int UserId { get; set; }
    public List<WishlistItem> WishlistItems { get; set; } = new();
}

public class WishlistItem
{
    public int Id { get; set; }
    public int WishlistId { get; set; }
    public Wishlist Wishlist { get; set; } = null!;
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    public DateTime AddedDate { get; set; } = DateTime.UtcNow;
}