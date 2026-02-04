using dotnet_store.models;
using dotnet_store.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dotnet_store.Controllers;

public class UserController : Controller
{
    private UserManager<AppUser> _userManager;
    private RoleManager<AppRole> _roleManager;
    public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public IActionResult Index()
    {
        return View(_userManager.Users);
    }

    public IActionResult Create()
    {
        var roles = _roleManager.Roles.ToList();
        ViewBag.RoleList = new SelectList(roles, "Name", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(UserCreateModel model)
    {
        if (ModelState.IsValid)
        {
            var randomNumber = new Random().Next(0, 10000);
            var username = $"{model.NameSurname.Replace(" ", "").ToLower()}{randomNumber}";
            var user = new AppUser
            {
                NameSurname = model.NameSurname,
                Email = model.Email,
                PhoneNumber = "+905" + model.PhoneNumber,
                UserName = username,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.SelectedRole);
                TempData["Message"] = $"User {user.UserName} has been added.";
                return RedirectToAction("Index");
            }
        }
        var roles = _roleManager.Roles.ToList();
        ViewBag.RoleList = new SelectList(roles, "Name", "Name");
        return View(model);
    }
}