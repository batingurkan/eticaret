using AutoMapper;
using eTicaretDAL;
using eTicaretEL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.ViewComponents;

public class LogOutBestSeller:ViewComponent
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public LogOutBestSeller(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public  async Task<IViewComponentResult> InvokeAsync()
    {
        var products =await _dataContext.Product
            .Include(p => p.Category)
            .ToListAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products);
        
    
        return View(productDtos);
    }
}