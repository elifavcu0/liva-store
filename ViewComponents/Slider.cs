using liva_store.Data;
using Microsoft.AspNetCore.Mvc;

namespace liva_store.ViewComponents;

public class Slider : ViewComponent
{
    private readonly DataContext _context;
    public Slider(DataContext context)
    {
        _context = context;
    }

    public IViewComponentResult Invoke()
    {

        return View(_context.Sliders.Where(i => i.IsActive).OrderBy(i => i.Index).ToList());
    }
}