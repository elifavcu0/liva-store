using Microsoft.AspNetCore.Mvc;
using liva_store.Data;

namespace liva_store.Controllers;

public class HomeController : Controller
{
    private readonly DataContext _context;
    public HomeController(DataContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var products = _context.Products.Where(p => p.IsActive && p.IsHome).OrderByDescending(i => i.DiscountRate).ToList();
        ViewData["Categories"] = _context.Categories.ToList();
        return View(products);
    }
}
