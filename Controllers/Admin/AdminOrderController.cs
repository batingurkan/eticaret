using AutoMapper;
using eTicaretDAL;
using eTicaretEL.Dtos;
using eTicaretEL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.Controllers.Admin;


public class AdminOrderController : Controller
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public AdminOrderController(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }


    public async Task<IActionResult> Index()
    {
        var list = await _dataContext.Order.Include(p => p.User).Include(p => p.OrderDetail).Include(p => p.OrderStatus)
            .Include(p => p.Cargo).ToListAsync();
        var orderDto = _mapper.Map<List<OrderDto>>(list);
        ViewBag.v1 = list.Count();
        return View(orderDto);
    }
    public async Task<IActionResult>  UserOrder(int id)
    {
        var list = await _dataContext.Order.Include(p => p.User).Include(p => p.OrderDetail).Include(p => p.OrderStatus)
            .Include(p => p.Cargo).Where(x => x.UserId == id).ToListAsync();
        var orderDto = _mapper.Map<List<OrderDto>>(list);
        ViewBag.v1 = list.Count();
        return View("Index", orderDto);
    }
    public async Task<IActionResult> OrderDetail(int id)
    {
        var list = await _dataContext.OrderDetail.Where(x=>x.OrderId==id).Include(p => p.Products).ToListAsync();
        var orderDto = _mapper.Map<List<OrderDetailDto>>(list);
        return View(orderDto);
    }
    
    public async Task<IActionResult> PendingApproval()
    {
        var list = await _dataContext.Order.Include(p => p.User).Include(p => p.OrderDetail).Include(p => p.OrderStatus)
            .Include(p => p.Cargo).Where(x => x.OrderStatus.Id == 1).ToListAsync();
        var orderDto = _mapper.Map<List<OrderDto>>(list);
        ViewBag.v1 = list.Count();
        return View("Index", orderDto);
    }

    public async Task<IActionResult> Shipped()
    {
        var list = await _dataContext.Order.Include(p => p.User).Include(p => p.OrderDetail).Include(p => p.OrderStatus)
            .Include(p => p.Cargo).Where(x => x.OrderStatus.Id == 3).ToListAsync();
        var orderDto = _mapper.Map<List<OrderDto>>(list);

        ViewBag.v1 = list.Count();
        return View("Index", orderDto);
    }

    public async Task<IActionResult> Approved()
    {
        var list = await _dataContext.Order.Include(p => p.User).Include(p => p.OrderDetail).Include(p => p.OrderStatus)
            .Include(p => p.Cargo).Where(x => x.OrderStatus.Id == 2).ToListAsync();
        var orderDto = _mapper.Map<List<OrderDto>>(list);
        ViewBag.v1 = list.Count();
        return View("Index", orderDto);
    }

    [HttpGet]
    public async Task<IActionResult> OrderEdit(int id)
    {
        List<SelectListItem> cargo = (from x in _dataContext.Cargo.ToList()
            select new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();
        List<SelectListItem> status = (from x in _dataContext.OrderStatusTypes.ToList()
            select new SelectListItem
            {
                Text = x.Status,
                Value = x.Id.ToString()
            }).ToList();
        
        ViewBag.car = cargo;
        ViewBag.stat = status;
        
        var list =await _dataContext.Order.FirstOrDefaultAsync(x=>x.Id==id);
        
        return View(list);
    }
    [HttpPost]
    public async Task<IActionResult> OrderEdit(Order p)
    {
        var order = _dataContext.Order.Where(x => x.Id == p.Id).FirstOrDefault();
        order.CargoId = p.CargoId;
        order.CargoNo = p.CargoNo;
        order.OrderDate = order.OrderDate;
        order.TotalAmount = p.TotalAmount;
        order.OrderStatusId = p.OrderStatusId;
        order.UserId = order.UserId;
      
        _dataContext.Order.Update(order);
        await _dataContext.SaveChangesAsync();

        return RedirectToAction("Index", "AdminOrder");
    }
}