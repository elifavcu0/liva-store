using dotnet_store.models;
using dotnet_store.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace dotnet_store.Controllers;

[Authorize(Roles = "Admin")]
public class UserController : Controller
{
    private UserManager<AppUser> _userManager;
    private RoleManager<AppRole> _roleManager;
    private IPasswordHasher<AppUser> _passwordHasher;
    public UserController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager, IPasswordHasher<AppUser> passwordHasher)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _passwordHasher = passwordHasher;
    }

    private async Task GetRoles()
    {
        ViewBag.RoleList = await _roleManager.Roles.Select(i => i.Name).ToListAsync();
    }

    public async Task<IActionResult> Index(string role)
    {
        var roles = _roleManager.Roles;
        ViewBag.RoleList = new SelectList(roles, "Name", "Name", role);

        if (role != null)
        {
            return View(await _userManager.GetUsersInRoleAsync(role));
        }
        return View(_userManager.Users);
    }

    public async Task<IActionResult> Create()
    {
        await GetRoles();
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
                TempData["Success"] = $"User {user.UserName} has been added.";
                return RedirectToAction("Index");
            }
        }
        await GetRoles();
        return View(model);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id != null)
        {
            var user = await _userManager.FindByIdAsync(id.ToString()!);
            if (user != null)
            {
                await GetRoles();

                var userRoles = await _userManager.GetRolesAsync(user);
                var allRoles = _roleManager.Roles.ToList();

                var model = new UserEditModel
                {
                    Id = user.Id,
                    NameSurname = user.NameSurname,
                    Email = user.Email!,
                    PhoneNumber = user.PhoneNumber!,
                    SelectedRoles = userRoles.ToList(),
                    Roles = allRoles
                };
                return View(model);
            }

            TempData["Error"] = "There's no such an user.";
            return RedirectToAction("Index");
        }
        TempData["Error"] = "Invalid operation.";
        return RedirectToAction("Index");
    }

    [HttpPost]

    public async Task<IActionResult> Edit(int? id, UserEditModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByIdAsync(id.ToString()!);
            if (user == null) return NotFound();

            user.NameSurname = model.NameSurname;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;

            // Edit user's new role

            var userRoles = await _userManager.GetRolesAsync(user);
            var selectedRoles = model.SelectedRoles ?? new List<string>(); //model.SelectedRoles null gelirse SequenceEqual patlayabilir, o yüzden garantiye aldık.
            bool isRoleChanged = !userRoles.OrderBy(x => x).SequenceEqual(selectedRoles.OrderBy(x => x)); // Alfabetik sırala, SequenceEqual ile tek tek iki listedeki elemanları kontrol et. Örn; eğer 1. elemanları birbirinden farklı gelirse burası direkt false olur.

            if (isRoleChanged)
            {
                if (userRoles.Any()) await _userManager.RemoveFromRolesAsync(user, userRoles);
                if (selectedRoles.Any())
                {
                    await _userManager.AddToRolesAsync(user, selectedRoles);
                }
            }

            if (!string.IsNullOrEmpty(model.Password))
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, model.Password);
                await _userManager.UpdateSecurityStampAsync(user); // Kullanıcının diğer cihazlarda bulunan oturumları da kapatılır.
            }
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                TempData["Success"] = $"User {user.UserName}'s been updated.";
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
        await GetRoles();
        TempData["Error"] = "Invalid operation.";
        return View(model);
    }

}