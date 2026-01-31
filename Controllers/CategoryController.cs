using dotnet_store.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace dotnet_store.Controllers;

public class CategoryController : Controller
{
    private readonly DataContext _context;
    public CategoryController(DataContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        // var categories = _context.Categories.Include(i => i.Products).ToList();  Kategorileri getirirken ürünleri de getir demiş olduk ancak performansı yorabilen bir işlemdir.Default olarak ürünler + kategoriler değil, sadece kategoriler getirilir.

        var categories = _context.Categories.Select(i => new CategoryGetModel
        {
            Id = i.Id,
            Name = i.Name,
            Url = i.Url,
            ProductQuantity = i.Products.Count
        }).ToList();

        return View(categories);
    }

    //Metodlar varsayılan olarak [HttpGet] metodudur, server tarafından bilgi almak için çağrılır.
    public IActionResult Create() // GET metodu
    {
        return View();
    }
    [HttpPost] // Bir altındaki metot "post" metodu olur (server tarafına bilgi gönderir).
    public IActionResult Create(string categoryName, string categoryUrl)
    {
        var entity = new Category
        {
            Name = categoryName,
            Url = categoryUrl
        };

        _context.Categories.Add(entity);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
}

/*
1- EF Core CategoryGetModel sınıfına bakar. Bu sınıf sadece Id, Name, Url ve ürün sayısına sahip olduğundan SQL'i ona göre yazar: SELECT Id, Name, Url, (Count...) FROM ...
2- Veritabanından sunucuya sadece bu 4 parça bilgi gelir.
3- Gelen veri doğrudan CategoryGetModel nesnesinin içine dolar.*/