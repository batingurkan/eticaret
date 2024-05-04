using AutoMapper;
using eTicaretDAL;
using eTicaretEL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.ViewComponents;

public class ProductSold:ViewComponent
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public ProductSold(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }


    
    public async Task<IViewComponentResult> InvokeAsync(int id)
    { 
        var userId = HttpContext.Session.GetInt32("userId");
        var user = await _dataContext.Order.Include(x=>x.OrderDetail).Where(x => x.UserId != userId)
            .Include(p => p.User.Products).Include(x=>x.OrderStatus)
            .Where(x=> x.OrderStatus.Status == "Onaylandı").ToListAsync();
        var userDto = _mapper.Map<List<OrderDto>>(user);
        ViewBag.v1 = user.Count();
        return View(userDto);
    }
}