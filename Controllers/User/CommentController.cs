using eTicaretDAL;
using eTicaretEL.Entities;
using Microsoft.AspNetCore.Mvc;

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
        decimal ratingValue = model.Raiting;
        
        await _dataContext.Comment.AddAsync(model);
        await _dataContext.SaveChangesAsync();

        return RedirectToAction("Index", "Home");
    }
}