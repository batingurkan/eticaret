using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTicaret.Controllers.Admin;

public class AdminController : Controller
{

    public IActionResult Index()
    {
        return View();
    }
}