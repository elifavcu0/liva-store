using dotnet_store.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_store.Controllers;

public class AccountController : Controller
{
    private UserManager<IdentityUser> _userManager;
    public AccountController(UserManager<IdentityUser> userManager)
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
            var user = new IdentityUser { UserName = model.Username, Email = model.Email };
            var result = await _userManager.CreateAsync(user,model.Password); // Güvenlik nedeniyle şifreyi burada gönderiyoruz, veritabanına özel şekilde kodlanmış (hashlanmış) şekilde kaydediliyor.

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