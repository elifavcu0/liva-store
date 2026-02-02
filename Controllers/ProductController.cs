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

    private void LoadCategories()
    {
        var categories = _context.Categories.ToList();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
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
                Image = fileName // upload
            };
            if (model.Name == null) return RedirectToAction("Edit");

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
            ImageName = i.Image

        }).FirstOrDefault(i => i.Id == id);

        LoadCategories();

        return View(entity);
    }

    [HttpPost]

    public async Task<IActionResult> Edit(int id, ProductEditModel model)
    {
        // if (model.ImageFile == null || model.ImageFile.Length == 0)
        // {
        //     ModelState.AddModelError("ImageName", "Image must be chosen.");
        // }
        if (ModelState.IsValid)
        {
            if (id != model.Id) return NotFound();

            if (model.Name == null)
            {
                TempData["Message"] = "Product name field cannot be empty.";
                return RedirectToAction("Edit");
            }

            var entity = _context.Products.Find(id);

            if (entity == null) return View(model);

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

            _context.SaveChanges();

            TempData["Message"] = $"{entity.Name} has been updated.";

            return RedirectToAction("Index");
        }
        LoadCategories();
        return View(model);
    }
}