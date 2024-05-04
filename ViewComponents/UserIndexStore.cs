using AutoMapper;
using eTicaretDAL;
using eTicaretEL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.ViewComponents;

public class UserIndexStore:ViewComponent
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public UserIndexStore(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<IViewComponentResult> InvokeAsync(int userId)
    {    var ıd = HttpContext.Session.GetInt32("userId");
        var products =await _dataContext.Product
            .Include(p => p.User)
            .Where(p => p.UserId == ıd && p.IsApproved==true)
            .ToListAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products);
    
        ViewBag.v1 = products.Count();
    
        return View(productDtos);
    }
}