using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_store.Controllers;

[Authorize]// sadece yetkilendirilmiş kişiler admin paneline erişebilir
public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}