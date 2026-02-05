using dotnet_store.Data;
using dotnet_store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_store.Controllers;

[Authorize(Roles = "Admin")]
public class SliderController : Controller
{
    private readonly DataContext _context;
    public SliderController(DataContext context)
    {
        _context = context;
    }
    public IActionResult Index()
    {
        var sliders = _context.Sliders.Select(slider => new SliderGetModel
        {
            Id = slider.Id,
            Title = slider.Title,
            Description = slider.Description,
            Image = slider.Image,
            IsActive = slider.IsActive,
            Index = slider.Index
        }).ToList();

        return View(sliders);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(SliderCreateModel model)
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

            int maxIndex = _context.Sliders.Max(slider => (int?)slider.Index) ?? 0; // Eğer tablo boşsa maxindex 0 olur.
            var entity = new Slider
            {
                Title = model.Title,
                Description = model.Description,
                Image = fileName,
                IsActive = model.IsActive,
                Index = maxIndex + 1
            };

            if (model.Title == null) return RedirectToAction("Create");

            _context.Sliders.Add(entity);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        TempData["Error"] = "Invalid operation, try again.";
        return View(model);
    }

    public IActionResult Edit(int id)
    {
        var slider = _context.Sliders.Where(i => i.Id == id).Select(slider => new SliderEditModel
        {
            Id = slider.Id,
            Title = slider.Title,
            Description = slider.Description,
            ImageName = slider.Image,
            IsActive = slider.IsActive,
            Index = slider.Index
        }).FirstOrDefault();

        if (slider == null)
        {
            TempData["Error"] = "The entity doesn't exist.";
            return RedirectToAction("Index");
        }

        return View(slider);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, SliderEditModel model)
    {
        if (ModelState.IsValid)
        {
            if (model.Id != id)
            {
                TempData["Error"] = "The slider not found.";
                return NotFound();
            }

            var entity = _context.Sliders.Find(id);

            if (entity == null)
            {
                TempData["Error"] = "The entity doesn't exist.";
                return View(model);
            }
            if (model.ImageFile != null)
            {
                var fileName = Path.GetRandomFileName() + ".jpg";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(stream);
                }
                entity.Image = fileName;
            }
            entity.Title = model.Title;
            entity.Description = model.Description;
            entity.IsActive = model.IsActive;

            _context.SaveChanges();

            TempData["Success"] = $"{entity.Title}'s been updated.";
            return RedirectToAction("Index");
        }
        TempData["Error"] = "An error occured.";

        return View(model);
    }

    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            TempData["Error"] = "The id is invalid.";
            return RedirectToAction("Index");
        }

        var entity = _context.Sliders.Find(id);
        if (entity == null)
        {
            TempData["Error"] = "The slider doesn't exist.";
            return RedirectToAction("Index");
        }
        return View(entity);
    }

    [HttpPost]
    public IActionResult DeleteConfirm(int? id)
    {
        if (id == null)
        {
            TempData["Error"] = "The id is invalid.";
            return RedirectToAction("Index");
        }
        var entity = _context.Sliders.Find(id);
        if (entity != null)
        {
            _context.Sliders.Remove(entity);
            _context.SaveChanges();

            TempData["Success"] = $"{entity.Title}'s been deleted.";
        }
        return RedirectToAction("Index");
    }
}