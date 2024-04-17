using AutoMapper;
using eTicaretDAL;
using eTicaretEL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.ViewComponents;

public class UserProfileStore:ViewComponent
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public UserProfileStore( DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }
    public async Task<IViewComponentResult> InvokeAsync(int userId)
    { 
    
        var products =await _dataContext.Product
            .Include(p => p.User)
            .Where(p => p.UserId == userId)
            .ToListAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products);
    
        ViewBag.v1 = products.Count();
    
        return View(productDtos);
    }
}