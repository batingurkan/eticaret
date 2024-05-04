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

    public async Task<IViewComponentResult> InvokeAsync(int orderId)
    {
        var orderDetails = await _dataContext.OrderDetail
            .Where(od => od.OrderId == orderId)
            .Include(od => od.Products)
            .ToListAsync();

        var orderDto = _mapper.Map<List<OrderDetailDto>>(orderDetails);
        return View(orderDto);
    }
}