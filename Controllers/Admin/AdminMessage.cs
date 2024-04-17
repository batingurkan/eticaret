using eTicaretDAL;
using eTicaretEL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace eTicaret.Controllers.Admin;

public class AdminMessage : Controller
{
    private readonly DataContext _dataContext;

    public AdminMessage(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public IActionResult Detail(int id)
    {
        var list = _dataContext.Message.FirstOrDefault(x=>x.Id==id);
        return View(list);
    }
    public IActionResult Sender()
    {
        var list = _dataContext.Message.Where(x=>x.SenderMail=="Admin").ToList();
        
        return View(list);
    }
    public IActionResult Reciver()
    {
        var list = _dataContext.Message.Where(x=>x.ReciverMail=="Admin").ToList();
        ViewBag.v1 = list.Count();
        return View("Sender",list);
    }
    [HttpGet]
    public IActionResult NewMessage()
    {
        return View();
    }
    [HttpPost]
    public IActionResult NewMessage(Message m)
    {

        m.Date = DateTime.Now;
        m.SenderMail = "Admin";
        
        _dataContext.Message.Add(m);
        _dataContext.SaveChanges();
        return RedirectToAction("Sender");
    }
}