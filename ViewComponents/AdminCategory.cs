using eTicaretDAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.ViewComponents;

public class AdminCategory:ViewComponent
{
    private readonly DataContext _dataContext;

    public AdminCategory(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IViewComponentResult> InvokeAsync(int page = 1)
    {
        var list =await _dataContext.Category.ToListAsync();
        int pageSize = 5; 

        var paginatedProducts = list
            .OrderBy(s => s.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        
        ViewBag.CurrentPage = page; 
        ViewBag.v1 = list.Count();
        return View(paginatedProducts);
    }
}