using AutoMapper;
using eTicaretDAL;
using eTicaretEL.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.Controllers;
[AllowAnonymous]
public class LogOutUserController : Controller
{
    private readonly DataContext _dataContext;
    private IMapper _mapper;

    public LogOutUserController(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<IActionResult> Index()
    {
        var list =await _dataContext.Product.ToListAsync();
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
        
        return View(productDtos);
    }
    public async Task<IActionResult> HomeLogOutCategory(int id)
    {
        var products = await _dataContext.Product
            .Include(p => p.Category).Where(x=>x.CategoryId==id)
            .ToListAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products);
        ViewBag.v1 = products.Count();
        
        return View("HomePage", productDtos);
    }

    public  async Task<IActionResult> Product(int id)
    {
        var product =await _dataContext.Product
            .Include(p => p.User)
            .FirstOrDefaultAsync(x => x.Id == id);

        var productDto = _mapper.Map<ProductDto>(product);

        return View(productDto);
    }

}