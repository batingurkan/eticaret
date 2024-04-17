using eTicaretDAL;
using eTicaretEL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.Controllers.User;

public class FavoriesController : Controller
{
    private readonly DataContext _dataContext;

    public FavoriesController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpPost]
    public async Task<IActionResult> FavorieAdd(Favories f, int favoriesId)
    {
        var userId = HttpContext.Session.GetInt32("userId");


        // Ürünün favorilerde olup olmadığını kontrol et
        var existingFavorite = await _dataContext.Favories
            .FirstOrDefaultAsync(x => x.UserId == userId && x.ProductId == favoriesId);

        if (existingFavorite != null)
        {
            // Eğer ürün zaten favorilerde ise işlemi gerçekleştirme
            return RedirectToAction("Favories", "User", new { id = userId.Value });
        }

        // Favori olarak eklenmemişse, yeni favori olarak ekle
        f.ProductId = favoriesId;
        f.UserId = userId.Value;
        await _dataContext.Favories.AddAsync(f);
        await _dataContext.SaveChangesAsync();

        return RedirectToAction("Favories", "User", new { id = userId.Value });
    }

    [HttpPost]
    public async Task<IActionResult> FavorieRemove(int favoriesId)
    {
        var userId = HttpContext.Session.GetInt32("userId");
        
        var fav = await _dataContext.Favories.FirstOrDefaultAsync(x => x.Id == favoriesId);

        if (fav != null)
        {
            _dataContext.Favories.Remove(fav);
            await _dataContext.SaveChangesAsync();
        }
        
        return RedirectToAction("Favories", "User", new { id = userId.Value });
    }
}