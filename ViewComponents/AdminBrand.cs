using eTicaretDAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.ViewComponents;

public class AdminBrand:ViewComponent
{
    private readonly DataContext _dataContext;

    public AdminBrand(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IViewComponentResult> InvokeAsync(int page = 1)
    {
        var list =await _dataContext.Brand.ToListAsync();
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