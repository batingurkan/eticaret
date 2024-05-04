using eTicaretDAL;
using eTicaretEL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.Controllers.User;

public class CommentController : Controller
{
    private readonly DataContext _dataContext;

    public CommentController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpGet]
    public IActionResult CommentAdd(int productId)
    {
        var comment = new Comment { ProductId = productId };
        return View(comment);
    }
    [HttpPost]
    public async Task<IActionResult> CommentAdd(Comment model, int productId)
    {
        
        var userId = HttpContext.Session.GetInt32("userId");

        model.UserId = userId.Value;
        model.Date = DateTime.Now;
        model.Description = model.Description;
        model.ProductId = productId;
        model.Raiting = model.Raiting;

        
        await _dataContext.Comment.AddAsync(model);
        await _dataContext.SaveChangesAsync();
        ViewBag.CommentSuccess = "Yorumunuz başarıyla kaydedildi.";
        return RedirectToAction("Index", "Home");
    }
    [HttpGet]
    public async Task<IActionResult> CommentEdit(int id)
    {
        
        var list = await _dataContext.Comment.Where(x => x.Id == id).FirstOrDefaultAsync();
        return View(list);
    }

    [HttpPost]
    public async Task<IActionResult> CommentEdit(Comment p)
    {
        var comment = _dataContext.Comment.Where(x => x.Id == p.Id).FirstOrDefault();
        comment.Description = p.Description;
        comment.Raiting = p.Raiting;
        comment.UserId = p.UserId;
        comment.Date = p.Date;
        comment.ProductId = p.ProductId;
        _dataContext.Comment.Update(comment);
        await _dataContext.SaveChangesAsync();

        return RedirectToAction("Index", "AdminComment");
    }
}