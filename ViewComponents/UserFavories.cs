using AutoMapper;
using eTicaretDAL;
using eTicaretEL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.ViewComponents;

public class UserFavories:ViewComponent
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public UserFavories(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var ıd = HttpContext.Session.GetInt32("userId");
        var products =await _dataContext.Favories
            .Include(p => p.Product).Include(p=>p.User)
            .Where(p => p.UserId == ıd)
            .ToListAsync();

        var productDtos = _mapper.Map<List<FavoriesDto>>(products);
    
        ViewBag.v1 = products.Count();
        ViewBag.CartId = ıd;
        return View(productDtos);
    }
}