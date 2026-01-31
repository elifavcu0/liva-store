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

    //http://localhost:5283/products/phone?q=apple
    //route params : url => value
    // query string : q => value
    public IActionResult List(string url, string q)
    {
        var query = _context.Products.Where(i => i.IsActive); // Queryable tipi => Lazım olduğunda çalıştırılabilecek tip


        if (!string.IsNullOrEmpty(url))
        {
            query = query.Where(i => i.Category.Url == url); // url mevcutken query'i kategoriye göre filtrele
        }
        if (!string.IsNullOrEmpty(q))
        {
            query = query.Where(i => i.Name.ToLower().Contains(q.ToLower()));
            ViewData["q"] = q;
        }

        return View(query.ToList());
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