using eTicaretDAL;
using eTicaretEL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.ViewComponents;

public class OrderSummary:ViewComponent
{
    private readonly DataContext _dataContext;

    public OrderSummary(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async  Task<IViewComponentResult> InvokeAsync(int id)
    {
        var list = await _dataContext.OrderDetail.Include(x=>x.Orders).Include(x=>x.Orders.OrderStatus).Where(x=>x.Id==id).FirstOrDefaultAsync();
        return View(list);
    }
    
}