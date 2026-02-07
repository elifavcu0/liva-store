using liva_store.Data;
using liva_store.Models;
using Microsoft.AspNetCore.Mvc;

namespace liva_store.ViewComponents;

public class Navbar : ViewComponent
{
    private readonly DataContext _context;
    public Navbar(DataContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {
        var Categories = _context.Categories.ToList();
        return View(Categories); // Default olarak Shared/Components/Navbar altındaki Default.cshtml çağrılır ancak farklı görünümlerin çağrılmasını istersek return View("Other",Categories); yazarak çağırabiliriz.
    }
}