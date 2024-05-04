using eTicaretDAL;
using eTicaretEL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.Controllers.Admin;

public class AdminCommentController : Controller
{
    private readonly DataContext _dataContext;

    public AdminCommentController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    
    public IActionResult Index()
    {
      
        var list = _dataContext.Comment.Include(x=>x.User).Include(x=>x.Product).ToList();
        @ViewBag.v1 = list.Count();
        return View(list);
    }
    

    public IActionResult UserComments(int id)
    {
      
        var list = _dataContext.Comment.Include(x=>x.User).Include(x=>x.Product).Where(x=>x.UserId==id).ToList();
        @ViewBag.v1 = list.Count();
        return View("Index",list);
    }
    public IActionResult ProductComments(int id)
    {
      
        var list = _dataContext.Comment.Include(x=>x.User).Include(x=>x.Product).Where(x=>x.ProductId==id).ToList();
        @ViewBag.v1 = list.Count();
        return View("Index",list);
    }

}