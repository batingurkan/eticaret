using AutoMapper;
using eTicaretDAL;
using eTicaretEL.Dtos;
using eTicaretEL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.ViewComponents;

public class HomePageBrand:ViewComponent
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public HomePageBrand(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
       var list =await _dataContext.Brand.ToListAsync();
       
        return View(list);
    }
    
}