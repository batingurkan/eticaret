using System.Diagnostics;
using AutoMapper;
using eTicaret.Models;
using eTicaretDAL;
using eTicaretEL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.Controllers.User;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public HomeController(ILogger<HomeController> logger, DataContext dataContext, IMapper mapper)
    {
        _logger = logger;
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
    public async Task<IActionResult> Index()
    { 
        var list = await _dataContext.Product.ToListAsync();
        PopulateDropdowns();
        return View(list);
    }
    public async Task<IActionResult> HomePage(int page = 1)
    {
        int pageSize = 9; // Sayfa başına ürün sayısı

        var products = await _dataContext.Product
            .Include(p => p.Category)
            .ToListAsync();

        var paginatedProducts = products
            .OrderBy(s => s.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.CurrentPage = page; // Şu anki sayfa numarasını ViewBag'e atayın
        ViewBag.v1 = products.Count();

        var productDtos = _mapper.Map<List<ProductDto>>(paginatedProducts);

        PopulateDropdowns();
        return View(productDtos);
    }


    public  async Task<IActionResult> Product(int id)
    {
        var product =await _dataContext.Product
            .Include(p => p.User)
            .FirstOrDefaultAsync(x => x.Id == id);

        var productDto = _mapper.Map<ProductDto>(product);
        PopulateDropdowns();
        return View(productDto);
    }
    
    public async Task<IActionResult> UserProfile(int id)
    {
        var user = await _dataContext.User
            .Include(p => p.Products)
            .FirstOrDefaultAsync(x => x.Id == id);

        var userDto = _mapper.Map<UserDto>(user);
        PopulateDropdowns();
        return View(userDto);
    }
    
    
    
    public async Task<IActionResult> Almostdone()
    {
        var products = await _dataContext.Product
            .Include(p => p.Category)
            .ToListAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products);
        ViewBag.v1 = products.Count();
        PopulateDropdowns();
        return View("HomePage", productDtos);
    }
    public async Task<IActionResult> HomeCategory(int id)
    {
        var products = await _dataContext.Product
            .Include(p => p.Category).Where(x=>x.CategoryId==id)
            .ToListAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products);
        ViewBag.v1 = products.Count();
        PopulateDropdowns();
        return View("HomePage", productDtos);
    }

    
    public async Task<IActionResult> CheapProducts()
    {
        var products = await _dataContext.Product
            .Include(p => p.Category)
            .OrderBy(p => p.Price)
            .ToListAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products);
        ViewBag.v1 = _dataContext.Product.Count();
        PopulateDropdowns();
        return View("HomePage", productDtos);
    }
    
    public async Task<IActionResult> BestSellers()
    {
        var users = await _dataContext.User
            .Include(p => p.Products)
            .ToListAsync();

        var userDtos = _mapper.Map<List<UserDto>>(users);
        PopulateDropdowns();
        return View( userDtos);
    }
    
    
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}