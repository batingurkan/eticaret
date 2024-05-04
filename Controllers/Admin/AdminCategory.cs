using Microsoft.AspNetCore.Mvc;

namespace eTicaret.Controllers.Admin;

public class AdminCategory : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}