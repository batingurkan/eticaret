using AutoMapper;
using eTicaretDAL;
using eTicaretEL.Dtos;
using eTicaretEL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.Controllers.User;

public class CartController : Controller
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;


    public CartController(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    private void PopulateDropdowns()
    {
        var user = _dataContext.User.FirstOrDefault();
        ViewBag.v4 = user.Email;
        var userId = HttpContext.Session.GetInt32("userId");
        ViewBag.fav = _dataContext.Favories.Where(x => x.UserId == userId).Count();
        ViewBag.Cart = _dataContext.CartItem.Where(x => x.UserId == userId).Count();
    
        var userCartProductIds = _dataContext.CartItem
            .Where(x => x.UserId == userId)
            .Select(x => x.ProductId)
            .ToList();
    
        ViewBag.Price = _dataContext.CartItem.Where(x => x.UserId == userId).Sum(x => x.Product.Price);
    
        ViewBag.Cargo = _dataContext.Increases
            .Include(x => x.Product)
            .Where(x => userCartProductIds.Contains(x.Product.Id))
            .Sum(x => x.ShippingMoney);
        
        ViewBag.Hizmet = _dataContext.Increases
            .Select(x => x.Comission)
            .FirstOrDefault();
    
        ViewBag.Total = ViewBag.Price + ViewBag.Cargo + ViewBag.Hizmet;
    }

    public async Task<IActionResult> Index(int id)
    {
        var userId = HttpContext.Session.GetInt32("userId");

        var cart = await _dataContext.CartItem.Include(p => p.User)
            .Include(p => p.Product).Where(x => x.UserId == userId)
            .ToListAsync();

        var cartItemDto = _mapper.Map<List<CartItemDto>>(cart);


        PopulateDropdowns();
        return View(cartItemDto);
    }

    [HttpPost]
    public async Task<IActionResult> CartItemAdd(int productId, int cartId)
    {
        var userId = HttpContext.Session.GetInt32("userId");
        ViewBag.Value = userId;

        var existingCartItem = await _dataContext.CartItem
            .FirstOrDefaultAsync(x => x.UserId == cartId && x.ProductId == productId);

        if (existingCartItem != null)
        {
            return RedirectToAction("Index", "Cart", new { id = userId });
        }


        var cartItem = new CartItem
        {
            ProductId = productId,
            UserId = cartId,
        };

        _dataContext.CartItem.Add(cartItem);
        await _dataContext.SaveChangesAsync();


        return RedirectToAction("Index", "Cart", new { id = userId });
    }

    [HttpPost]
    public async Task<IActionResult> CartRemove(int cartId)
    {
        var userId = HttpContext.Session.GetInt32("userId");

        var cart = await _dataContext.CartItem.FirstOrDefaultAsync(x => x.Id == cartId);

        if (cart != null)
        {
            _dataContext.CartItem.Remove(cart);
            await _dataContext.SaveChangesAsync();
        }

        return RedirectToAction("Index", "Cart", new { id = userId.Value });
    }
[HttpGet]
    public async Task<IActionResult> AddAdress()
    {
        
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> AddAdress(UserDetail detail)
    {
        var userId = HttpContext.Session.GetInt32("userId");

        var userDetail = new UserDetail
        { 
            Name = detail.Name,
            Adress = detail.Adress,
            Adress2 = detail.Adress2,
            Tel = detail.Tel,
            UserId = userId.Value
        };

        _dataContext.UserDetail.Add(userDetail);
        _dataContext.SaveChanges();

        return RedirectToAction("Payment","Cart");
    }
    public async Task<IActionResult> Payment(int id)
    {
        var userId = HttpContext.Session.GetInt32("userId");

        var cart = await _dataContext.CartItem.Include(p => p.User)
            .Include(p => p.Product).Where(x => x.UserId == userId)
            .ToListAsync();

        var cartItemDto = _mapper.Map<List<CartItemDto>>(cart);


        PopulateDropdowns();
        return View(cartItemDto);
    }
}