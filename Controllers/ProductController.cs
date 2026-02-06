using dotnet_store.Data;
using dotnet_store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace dotnet_store.Controllers;

[Authorize(Roles = "Admin")]
public class ProductController : Controller
{
    // Dependency Injection => DI
    private readonly DataContext _context;
    public ProductController(DataContext context) // veritabanından veri çekecek olan DataContext sınıfı türünde bir nesne oluşturduk.
    {
        _context = context;
    }

    private void LoadCategories()
    {
        var categories = _context.Categories.ToList();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
    }

    public IActionResult Index(int? category)
    {
        var query = _context.Products.AsQueryable();

        if (category != null)
        {
            query = query.Where(i => i.CategoryId == category);
        }

        LoadCategories();
        var products = query.Select(i => new ProductGetModel()
        {
            Id = i.Id,
            Name = i.Name,
            Price = i.Price,
            IsActive = i.IsActive,
            Image = i.Image,
            IsHome = i.IsHome,
            Category = i.Category.Name,
            DiscountRate = i.DiscountRate
        }).ToList();
        ViewBag.Categories = new SelectList(_context.Categories.ToList(), "Id", "Name", category);
        return View(products);
    }

    //http://localhost:5283/products/phone?q=apple
    //route params : url => value
    // query string : q => value

    [AllowAnonymous] // Yetkilendirmeye gerek yok, herkes görebilir.
    public IActionResult List(string url, string q)
    {
        var query = _context.Products.AsQueryable(); // Queryable tipi => Lazım olduğunda çalıştırılabilecek tip


        if (!string.IsNullOrEmpty(url))
        {
            query = query.Where(i => i.Category.Url == url); // url mevcutken query'i kategoriye göre filtrele
        }
        if (!string.IsNullOrEmpty(q))
        {
            query = query.Where(i => i.Name.ToLower().Contains(q.ToLower()));
            ViewData["q"] = q;
        }
        query = query.OrderByDescending(i => i.IsActive);
        return View(query.ToList());
    }

    [AllowAnonymous]
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
        LoadCategories();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCreateModel model) // Create([Bind("Name","Description")] ProductCreateModel model) şeklinde yazarsak sadece Name ve Description alanları gelir.
    {
        if (model.ImageFile == null || model.ImageFile.Length == 0)
        {
            ModelState.AddModelError("Image", "Image must be chosen.");
        }

        if (ModelState.IsValid)
        {
            var fileName = Path.GetRandomFileName() + ".jpg";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await model.ImageFile!.CopyToAsync(stream);
            }

            var entity = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price ?? 0,
                IsActive = model.IsActive,
                IsHome = model.IsHome,
                CategoryId = (int)model.CategoryId!,
                Image = fileName,
                DiscountRate = model.DiscountRate
            };

            _context.Products.Add(entity);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        LoadCategories();
        return View(model);
    }

    public IActionResult Edit(int id)
    {
        var entity = _context.Products.Select(i => new ProductEditModel
        {
            Id = i.Id,
            Name = i.Name,
            Description = i.Description,
            Price = i.Price,
            IsActive = i.IsActive,
            IsHome = i.IsHome,
            CategoryId = i.CategoryId,
            ImageName = i.Image,
            DiscountRate = i.DiscountRate

        }).FirstOrDefault(i => i.Id == id);

        LoadCategories();

        return View(entity);
    }

    [HttpPost]

    public async Task<IActionResult> Edit(int id, ProductEditModel model)
    {
        if (id != model.Id) return NotFound();

        if (ModelState.IsValid)
        {
            var entity = _context.Products.Find(id);

            if (entity == null) return NotFound();

            if (model.ImageFile != null) // Eğer yeni bir dosya seçildiyse
            {
                var fileName = Path.GetRandomFileName() + ".jpeg";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }
                entity.Image = fileName;
            }

            entity.Name = model.Name;
            entity.Price = model.Price ?? 0;
            entity.Description = model.Description;
            entity.CategoryId = (int)model.CategoryId!;
            entity.IsActive = model.IsActive;
            entity.IsHome = model.IsHome;
            entity.DiscountRate = model.DiscountRate;

            _context.SaveChanges();
            TempData["Success"] = $"{entity.Name} has been updated.";
            return RedirectToAction("Index");
        }
        TempData["Error"] = "Update failed.";
        LoadCategories();
        return View(model);
    }

    public IActionResult Delete(int? id)
    {
        if (id == null) return RedirectToAction("Index");

        var entity = _context.Products.Find(id);

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

        var entity = _context.Products.Find(id);

        if (entity != null)
        {
            _context.Products.Remove(entity);
            _context.SaveChanges();
            TempData["Success"] = $"{entity.Name}'s been deleted.";
        }
        return RedirectToAction("Index");
    }
}