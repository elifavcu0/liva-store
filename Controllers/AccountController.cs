using dotnet_store.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_store.Controllers;

public class AccountController : Controller
{
    private UserManager<AppUser> _userManager;
    private SignInManager<AppUser> _signInManager;
    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(AccountLoginModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email); // mail adresine göre DB'den kullanıcı bulunacak

            if (user != null) // önce mail ile kontrol sağlandı, kullanıcı mevcutsa şifre kontrolü yapılır.
            {
                await _signInManager.SignOutAsync(); //Tarayıcıda daha önceden kalma, süresi bitmiş, bozuk veya başka bir kullanıcıya ait bir "Kimlik Çerezi" (Cookie) kalmış olabilir, önce her şeyi temizleriz.
                var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, true);

                if (result.Succeeded)
                {
                    await _userManager.ResetAccessFailedCountAsync(user);
                    await _userManager.SetLockoutEndDateAsync(user, null);
                    return RedirectToAction("Index", "Home");
                }
                else if (result.IsLockedOut)
                {
                    var lockOutDate = await _userManager.GetLockoutEndDateAsync(user);
                    var timeLeft = lockOutDate.Value - DateTime.UtcNow;
                    ModelState.AddModelError("", $"Your account's been locked for {timeLeft.Minutes + 1} minutes, please wait.");
                }
                else ModelState.AddModelError("Password", "The password is incorrect.");
            }

            else
            {
                ModelState.AddModelError("Email", "No user was found registered with this email address.");
            }
        }
        return View(model);
    }

}