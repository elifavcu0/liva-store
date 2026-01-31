using dotnet_store.Models;
using Microsoft.AspNetCore.Mvc;
namespace dotnet_store.Controllers;

public class ProductController : Controller
{
    // Dependency Injection => DI
    private readonly DataContext _context;
    public ProductController(DataContext context) // veritabanından veri çekecek olan DataContext sınıfı türünde bir nesne oluşturduk.
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult List(string url)
    {
        // var products = _context.Products.ToList();

        List<Product> products = _context.Products.Where(i => i.Category.Url == url).ToList(); // SELECT * FROM Products // DataContext.cs dosyasındaki "public DbSet<Product> Products { get; set; }" satırına gider
        return View(products);
    }

    public IActionResult Details(int id)
    {
        // var product = _context.Products.FirstOrDefault(p => p.Id == id); 
        var product = _context.Products.Find(id);
        if (product == null) return RedirectToAction("Index", "Home"); // Ürün yoksa Home controller altında bulunan Index sayfasına yönlendirir.
        ViewData["SimilarProducts"] = _context.Products.Where(i => i.IsActive && i.CategoryId == product.CategoryId && i.Id != id).Take(4).ToList(); // 4 tane benzer ürünü al.
        return View(product);
    }
}