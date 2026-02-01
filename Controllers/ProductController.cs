using System.ComponentModel.Design;
using dotnet_store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        var products = _context.Products.Select(i => new ProductGetModel()
        {
            Id = i.Id,
            Name = i.Name,
            Price = i.Price,
            IsActive = i.IsActive,
            Image = i.Image,
            IsHome = i.IsHome,
            Category = i.Category.Name
        }).ToList();
        return View(products);
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
    public IActionResult Create()
    {
        //ViewBag.Categories = _context.Categories.ToList();
        ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name");
        return View();
    }

    [HttpPost]
    public IActionResult Create(ProductCreateModel model) // Create([Bind("Name","Description")] ProductCreateModel model) şeklinde yazarsak sadece Name ve Description alanları gelir.
    {
        var entity = new Product
        {
            Name = model.Name,
            Description = model.Description,
            Price = model.Price,
            IsActive = model.IsActive,
            IsHome = model.IsHome,
            CategoryId = model.CategoryId,
            Image = "1.jpeg" // upload
        };
        if (model.Name == null) return RedirectToAction("Index");
        _context.Products.Add(entity);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }


}