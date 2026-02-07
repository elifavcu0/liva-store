using System.IO.Compression;
using liva_store.Models;
using liva_store.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace liva_store.Controllers;

[Authorize(Roles = "Admin")]
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
    public IActionResult Create(CategoryCreateModel model)
    {
        if (ModelState.IsValid)
        {
            var entity = new Category
            {
                Name = model.Name,
                Url = model.Url
            };

            if (model.Name == null || model.Url == null) return RedirectToAction("Index");

            _context.Categories.Add(entity);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        return View(model);
    }

    public IActionResult Edit(int id)
    {
        var entity = _context.Categories.Select(i => new CategoryEditModel
        {
            Id = i.Id,
            Name = i.Name,
            Url = i.Url

        }).FirstOrDefault(i => i.Id == id);

        return View(entity);
    }

    [HttpPost]
    public IActionResult Edit(int id, CategoryEditModel model)
    {
        if (ModelState.IsValid)
        {
            if (id != model.Id) return NotFound();
            if (model.Name == null || model.Url == null) return RedirectToAction("Edit");

            var entity = _context.Categories.Find(id);

            if (entity == null) return NotFound();

            entity.Name = model.Name;
            entity.Url = model.Url;

            _context.SaveChanges();

            TempData["Success"] = $"{entity.Name} category has been updated.";

            return RedirectToAction("Index");
        }
        return View(model);
    }


    public IActionResult Delete(int? id)
    {
        if (id == null) return RedirectToAction("Index");

        var entity = _context.Categories.Find(id);

        if (entity != null)
        {
            return View(entity);
        }

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult DeleteConfirm(int? id)
    {
        if (id == null) return RedirectToAction("Index");

        var entity = _context.Categories.Find(id);

        if (entity != null)
        {
            _context.Categories.Remove(entity);
            _context.SaveChanges();
            TempData["Success"] = $"{entity.Name}'s been deleted.";
        }

        return RedirectToAction("Index");
    }
}

/*
1- EF Core CategoryGetModel sınıfına bakar. Bu sınıf sadece Id, Name, Url ve ürün sayısına sahip olduğundan SQL'i ona göre yazar: SELECT Id, Name, Url, (Count...) FROM ...
2- Veritabanından sunucuya sadece bu 4 parça bilgi gelir.
3- Gelen veri doğrudan CategoryGetModel nesnesinin içine dolar.*/