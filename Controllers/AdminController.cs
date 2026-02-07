using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace liva_store.Controllers;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}