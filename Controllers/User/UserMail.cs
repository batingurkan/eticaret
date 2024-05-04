using eTicaretDAL;
using eTicaretEL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace eTicaret.Controllers.User;

public class UserMail : Controller
{
    private readonly DataContext _dataContext;

    public UserMail(DataContext dataContext)
    {
        _dataContext = dataContext;
    }
    private void PopulateDropdowns()
    {
        var user = _dataContext.User.FirstOrDefault();
        ViewBag.v4= user.Email;
        var userId = HttpContext.Session.GetInt32("userId");
        ViewBag.fav = _dataContext.Favories.Where(x=>x.UserId==userId).Count();
        ViewBag.Cart=_dataContext.CartItem.Where(x=>x.UserId==userId).Count();
        ViewBag.Price = _dataContext.CartItem.Where(x => x.UserId == userId).Sum(x=>x.Product.Price);
        ViewBag.Cargo = 35;
        ViewBag.Hizmet = 20;
        ViewBag.Total = (ViewBag.Price) + (ViewBag.Cargo)+(ViewBag.Hizmet);
    }


    public IActionResult Detail(int id)
    {
        var list = _dataContext.Message.FirstOrDefault(x=>x.Id==id);
        PopulateDropdowns();
        return View(list);
    }

    public IActionResult Sender()
    {
        var usermail = HttpContext.Session.GetString("usermail");
        var list = _dataContext.Message.Where(x => x.SenderMail == usermail).ToList();
        ViewBag.v1 = list.Count();
        PopulateDropdowns();
        return View(list);
    }

    public IActionResult Reciver()
    {
        var usermail = HttpContext.Session.GetString("usermail");
        var list = _dataContext.Message.Where(x => x.ReciverMail == usermail).ToList();
        
        PopulateDropdowns();
        return View("Sender",list);
    }
    [HttpGet]
    public IActionResult NewMessage()
    {
        PopulateDropdowns();
        return View();
    }
    [HttpPost]
    public IActionResult NewMessage(Message m)
    {
        var usermail = HttpContext.Session.GetString("usermail");

        m.Date = DateTime.Now;
        m.SenderMail = usermail;
        m.Status = true;
        m.ReciverMail = "Admin";
        
        _dataContext.Message.Add(m);
        _dataContext.SaveChanges();
        PopulateDropdowns();
        return RedirectToAction("Sender");
    }
}