using AutoMapper;
using eTicaretDAL;
using eTicaretEL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.ViewComponents;

public class UserOrdersShipped:ViewComponent
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public UserOrdersShipped(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var userId = HttpContext.Session.GetInt32("userId");
        var list = await _dataContext.Order.Include(p => p.User).Include(p => p.OrderDetail).Include(p => p.OrderStatus)
            .Include(p => p.Cargo).Where(x => x.OrderStatus.Id == 3 && x.UserId==userId).ToListAsync();
        var orderDto = _mapper.Map<List<OrderDto>>(list);
        ViewBag.v1 = list.Count();
        return View(orderDto);
    }
    
}