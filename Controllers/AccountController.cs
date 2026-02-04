using System.Security.Claims;
using dotnet_store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
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

    public IActionResult SignIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SignIn(AccountLoginModel model, string? returnUrl)
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

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
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

    [Authorize]
    public async Task<IActionResult> Signout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("SignIn", "Account");
    }

    [Authorize]
    public IActionResult Settings()
    {
        return View();
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    public async Task<IActionResult> EditUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Uygulamaya giriş yapan user id'sini alır.
        var user = await _userManager.FindByIdAsync(userId!);

        if (user == null)
        {
            return RedirectToAction("SignIn", "Account");
        }
        return View(new AccountEditUserModel
        {
            NameSurname = user.NameSurname,
            Email = user.Email!,
            PhoneNumber = user.PhoneNumber!
        });
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> EditUser(AccountEditUserModel model)
    {
        if (ModelState.IsValid)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(userId!);

            if (user != null)
            {
                user.Email = model.Email;
                user.NameSurname = model.NameSurname;
                user.PhoneNumber = model.PhoneNumber;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    TempData["Success"] = "Your information's been updated.";
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

            }
            else TempData["Error"] = "Unsuccessful operation, try again.";
        }
        return View(model);
    }

    [Authorize]
    public async Task<IActionResult> ChangePassword()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _userManager.FindByIdAsync(userId!);

        if (user == null)
        {
            return RedirectToAction("SignIn", "Account");
        }
        return View();
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> ChangePassword(AccountChangePasswordModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User); //O anki oturum açmış kullanıcıyı böyle de bulabiliriz.

            if (user == null) return RedirectToAction("SignIn", "Account");

            bool isCurrentPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.CurrentPassword!);
            if (!isCurrentPasswordCorrect)
            {
                ModelState.AddModelError("CurrentPassword", "Your current password is incorrect.");
            }
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword!, model.NewPassword!);

            if (result.Succeeded)
            {
                TempData["Success"] = "Your password's been changed.";
                return RedirectToAction( "ChangePassword","Account");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(model);
    }
}