using System.Security.Claims;
using dotnet_store.Data;
using dotnet_store.Models;
using dotnet_store.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace dotnet_store.Controllers;

public class AccountController : Controller
{
    private readonly DataContext _context;
    private UserManager<AppUser> _userManager;
    private SignInManager<AppUser> _signInManager;
    private IEmailService _emailService;
    private ICartService _cartService;
    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IEmailService emailService, DataContext context, ICartService cartService)
    {
        _context = context;
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _cartService = cartService;
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

                    await _cartService.TransferCartToUser(user.UserName!);

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

            bool isCurrentPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.CurrentPassword);
            if (!isCurrentPasswordCorrect)
            {
                ModelState.AddModelError("CurrentPassword", "Your current password is incorrect.");
            }
            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

            if (result.Succeeded)
            {
                TempData["Success"] = "Your password's been changed.";
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(model);
    }

    //*******************************SONRA DEVAM ET*************************
    // public async Task<IActionResult> AddAddress()
    // {
    //     var user = await _userManager.GetUserAsync(User);
    //     if (user == null) return NotFound();
    //     return View();
    // }

    // [HttpPost]
    // [Authorize]
    // public async Task<IActionResult> AddAddress(AccountAddressModel model)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         var user = await _userManager.GetUserAsync(User);
    //         if (user == null) return NotFound();

    //         var address = new Address
    //         {
    //             Title = model.AddressTitle,
    //             City = model.City,
    //             District = model.District,
    //             OpenAddress = model.OpenAddress!,
    //             AppUserId = user.Id
    //         };
    //         _context.Addresses.Add(address);
    //         await _context.SaveChangesAsync();
    //         TempData["Success"] = "Your address's been saved.";
    //         //return RedirectToAction("Index");
    //     }
    //     return View(model);
    // }****************************************************************************

    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> ForgotPassword(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            TempData["Info"] = "Enter your email.";
            return View();
        }

        var user = await _userManager.FindByEmailAsync(email); // Girilen email ile kayıtlı kullanıcı var mı?
        if (user == null)
        {
            TempData["Error"] = "No user registered with this address was found.";
            return View();
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var url = Url.Action("ResetPassword", "Account", new { userId = user.Id, token });
        var link = $"<a href = 'http://localhost:5283{url}'>Reset Password</a>";

        await _emailService.SendEmailAsync(user.Email!, "Reset your password", link);

        TempData["Info"] = "Reset link has been sent to your email address.";
        return RedirectToAction("SignIn");
    }

    public async Task<IActionResult> ResetPassword(string userId, string token)
    {
        if (userId == null || token == null)
        {
            TempData["Error"] = "Invalid user or token.";
            return RedirectToAction("SignIn");
        }
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            TempData["Error"] = "No user was found.";
            return RedirectToAction("SignIn");
        }

        var model = new AccountResetPasswordModel
        {
            Token = token,
            Email = user.Email!
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> ResetPassword(AccountResetPasswordModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                TempData["Error"] = "No user was found.";
                return RedirectToAction("SignIn");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                TempData["Success"] = "Your password's been reset.";
                return RedirectToAction("SignIn");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        return View(model);
    }
}