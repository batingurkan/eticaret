using AutoMapper;
using eTicaretDAL;
using eTicaretEL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.Controllers.User;

public class UserController : Controller
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public UserController(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }
    
    private void PopulateDropdowns()
    {
        var user = _dataContext.User.FirstOrDefault();
        ViewBag.v4= user.Email;
        var userId = HttpContext.Session.GetInt32("userId");
        ViewBag.fav = _dataContext.Favories.Where(x=>x.UserId==userId).Count();
        ViewBag.Cart=_dataContext.CartItem.Where(x=>x.UserId==userId).Count();
        ViewBag.CartId = userId;
        
    }
    
    public async Task<IActionResult> Index(int id)
    {
       
        if (!HttpContext.User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Login");
        }

        if (id == 0 || !HttpContext.Session.GetInt32("userId").HasValue)
        {
            return RedirectToAction("Index", "Login"); // Redirect to login
        }

        var userId = HttpContext.Session.GetInt32("userId");

        var user = await _dataContext.User
            .Include(p => p.Products)
            .FirstOrDefaultAsync(x => x.Id == userId);

        var userDto = _mapper.Map<UserDto>(user);
        PopulateDropdowns();
        return View(userDto);
    }
    
    public async Task<IActionResult> Favories(int id)
    {
       
        if (!HttpContext.User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Login");
        }

        if (id == 0 || !HttpContext.Session.GetInt32("userId").HasValue)
        {
            return RedirectToAction("Index", "Login"); // Redirect to login
        }

        var userId = HttpContext.Session.GetInt32("userId");

        var user = await _dataContext.User
            .Include(p => p.Products)
            .FirstOrDefaultAsync(x => x.Id == userId);

        var userDto = _mapper.Map<UserDto>(user);
        PopulateDropdowns();
        return View(userDto);
    }
    
    public async Task<IActionResult> Orders(int id)
    {

        var userId = HttpContext.Session.GetInt32("userId");
        var user = await _dataContext.User
            .Include(p => p.Products)
            .FirstOrDefaultAsync(x => x.Id == userId);

        var userDto = _mapper.Map<UserDto>(user);
        PopulateDropdowns();
        return View(userDto);
    }
    public async Task<IActionResult> PendingApproval(int id)
    {

        var userId = HttpContext.Session.GetInt32("userId");
        var user = await _dataContext.User
            .Include(p => p.Products)
            .FirstOrDefaultAsync(x => x.Id == userId);

        var userDto = _mapper.Map<UserDto>(user);
        PopulateDropdowns();
        return View(userDto);
    }
    public async Task<IActionResult> Shipped(int id)
    {

        var userId = HttpContext.Session.GetInt32("userId");
        var user = await _dataContext.User
            .Include(p => p.Products)
            .FirstOrDefaultAsync(x => x.Id == userId);

        var userDto = _mapper.Map<UserDto>(user);
        PopulateDropdowns();
        return View(userDto);
    }
    public async Task<IActionResult> Approved(int id)
    {

        var userId = HttpContext.Session.GetInt32("userId");
        var user = await _dataContext.User
            .Include(p => p.Products)
            .FirstOrDefaultAsync(x => x.Id == userId);

        var userDto = _mapper.Map<UserDto>(user);
        PopulateDropdowns();
        return View(userDto);
    }
    public async Task<IActionResult> OrderDetail(int id)
    {
        var userId = HttpContext.Session.GetInt32("userId");
        var user = await _dataContext.OrderDetail
            .Include(p => p.Products)
            .Include(p => p.Orders)
            .FirstOrDefaultAsync(x => x.Id == id && x.Orders.UserId == userId);

        var orderDto = _mapper.Map<OrderDetailDto>(user);
        PopulateDropdowns();
        return View(orderDto);
    }
    public async Task<IActionResult> ApprovedProducts(int id)
    {

        var userId = HttpContext.Session.GetInt32("userId");
        var user = await _dataContext.User
            .Include(p => p.Products)
            .FirstOrDefaultAsync(x => x.Id == userId);

        var userDto = _mapper.Map<UserDto>(user);
        PopulateDropdowns();
        return View(userDto);
    }
   
}