using eTicaretDAL;
using eTicaretEL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        var list = _dataContext.Message.Where(x=>x.SenderMail=="Admin" && x.Status == true).ToList();
        
        return View(list);
    }
    public IActionResult Reciver()
    {
        var list = _dataContext.Message.Where(x=>x.ReciverMail=="Admin"  && x.Status == true).ToList();
        ViewBag.v1 = list.Count();
        return View("Sender",list);
    }
    public IActionResult Deleted()
    {
        var list = _dataContext.Message
            .Where(x => (x.ReciverMail == "Admin" || x.SenderMail == "Admin") && x.Status == false)
            .ToList();
        ViewBag.v1 = list.Count();
        return View(list);
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
        m.Status = true;
        
        _dataContext.Message.Add(m);
        _dataContext.SaveChanges();
        return RedirectToAction("Sender");
    }
    
    [HttpGet]
    public IActionResult NewMessageUser(int id)
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult NewMessageUser(Message m)
    {

        m.Date = DateTime.Now;
        m.SenderMail = "Admin";
        m.Status = true;
        
        _dataContext.Message.Add(m);
        _dataContext.SaveChanges();
        return RedirectToAction("Sender");
    }

    
    [HttpPost]
    public async Task<IActionResult> FalseMessage(int Id)
    {
        var message = await _dataContext.Message.FirstOrDefaultAsync(x => x.Id == Id);

        if (message == null)
        {
            return NotFound();
        }

        message.Status = false;
        _dataContext.Message.Update(message);
        await _dataContext.SaveChangesAsync();

        return RedirectToAction("Sender");
    }
    
    public async Task<IActionResult> DeleteMessage(int Id)
    {
        var message = await _dataContext.Message.FirstOrDefaultAsync(x => x.Id == Id);
        _dataContext.Message.Remove(message);
        await _dataContext.SaveChangesAsync();
        return RedirectToAction("Sender");
    }
}