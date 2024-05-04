using eTicaretDAL;
using eTicaretEL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace eTicaret.Controllers.Admin;

public class AdminIncreases : Controller
{
    private readonly DataContext _dataContext;

    public AdminIncreases(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public IActionResult Index()
    {
        var list = _dataContext.Increases.ToList();
        return View(list);
    }

    [HttpGet]
    public IActionResult IncreasesEdit(int id)
    {
        var list = _dataContext.Increases.FirstOrDefault(x => x.Id == id);
        return View(list);
    }
    [HttpPost]
    public IActionResult IncreasesEdit(Increases increases)
    {
        _dataContext.Increases.Update(increases);
        _dataContext.SaveChanges();
        return RedirectToAction("Index");
    }
}