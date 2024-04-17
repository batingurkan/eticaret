using AutoMapper;
using eTicaretDAL;
using eTicaretEL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.ViewComponents;

public class UserOrderDetail:ViewComponent
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public UserOrderDetail(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<IViewComponentResult> InvokeAsync(int id)
    {
        var userId = HttpContext.Session.GetInt32("userId");
    
        var orderDetails = await _dataContext.OrderDetail
            .Where(od => od.OrderId == userId )
            .Include(od => od.Products)
            .ToListAsync();

        var orderDetailDto = _mapper.Map<List<OrderDetailDto>>(orderDetails);
        return View(orderDetailDto); 
    }
    
}