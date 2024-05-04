using eTicaretDAL;
using Microsoft.AspNetCore.Mvc;

namespace eTicaret.ViewComponents;

public class UserDetail:ViewComponent
{
    private readonly DataContext _dataContext;

    public UserDetail(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IViewComponentResult> InvokeAsync(int id)
    {
        var userId = HttpContext.Session.GetInt32("userId");
        var list = _dataContext.UserDetail.Where(x=>x.UserId==userId).FirstOrDefault();
        return View(list);
    }
}