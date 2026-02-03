using dotnet_store.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_store.Controllers;

public class AccountController : Controller
{
    private UserManager<AppUser> _userManager;
    public AccountController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(AccountCreateModel model)
    {
        if (ModelState.IsValid)
        {
            int randomNumber = new Random().Next(0, 10000);
            string generatedUserName = $"{model.NameSurname.Replace(" ", "").ToLower()}{randomNumber}";

            var user = new AppUser { UserName = generatedUserName, Email = model.Email, NameSurname = model.NameSurname, PhoneNumber = "+905" + model.PhoneNumber };
            var result = await _userManager.CreateAsync(user, model.Password); // Güvenlik nedeniyle şifreyi burada gönderiyoruz, veritabanına özel şekilde kodlanmış (hashlanmış) şekilde kaydediliyor.

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(model);
    }
}