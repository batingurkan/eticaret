using AutoMapper;
using eTicaretDAL;
using eTicaretEL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.ViewComponents;

public class ProductSuggested:ViewComponent
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public ProductSuggested( DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }
    public async  Task<IViewComponentResult> InvokeAsync(int id)
    {
        var products = await _dataContext.Product
            .Include(p => p.Category).Take(4)
            .ToListAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products);
    
        return View(productDtos);
    }
  
}
