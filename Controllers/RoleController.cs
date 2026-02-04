using dotnet_store.models;
using dotnet_store.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_store.Controllers;

public class RoleController : Controller
{

    private RoleManager<AppRole> _roleManager;

    public RoleController(RoleManager<AppRole> roleManager)
    {
        _roleManager = roleManager;
    }
    public IActionResult Index()
    {
        return View(_roleManager.Roles);
    }
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(RoleCreateModel model)
    {
        if (ModelState.IsValid)
        {
            var role = new AppRole { Name = model.Name };
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(model);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var entity = await _roleManager.FindByIdAsync(id.ToString()); // id ile eşleşen rolü veritabanından bulur. (bu metod string aldığı için id'i string yaptık)
        if (entity != null)
        {
            return View(new RoleEditModel { Id = entity.Id, Name = entity.Name! });
        }
        TempData["Message"] = "The role was not found.";
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, RoleEditModel model)
    {
        if (ModelState.IsValid)
        {
            if (id != model.Id)
            {
                TempData["Message"] = "Invalid operation.";
                return RedirectToAction("Index");
            }
            var entity = await _roleManager.FindByIdAsync(id.ToString());

            if (entity != null)
            {
                entity.Name = model.Name;
                var result = await _roleManager.UpdateAsync(entity);

                if (result.Succeeded)
                {
                    TempData["Message"] = $"{entity.Name}'s been updated.";
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors) ModelState.AddModelError("", error.Description);
            }
        }
        TempData["Message"] = "The role was not found.";
        return View(model);
    }
}