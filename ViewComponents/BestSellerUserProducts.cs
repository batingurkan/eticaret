using AutoMapper;
using eTicaretDAL;
using eTicaretEL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.ViewComponents;

public class BestSellerUserProducts : ViewComponent
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public BestSellerUserProducts(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<IViewComponentResult> InvokeAsync(string username)
    {
        var user = await _dataContext.User.FirstOrDefaultAsync(u => u.Name == username);
        if (user == null)
        {
            // Kullanıcı bulunamadı, boş liste döndür
            return View(new List<ProductDto>());
        }

        var products = await _dataContext.Product
            .Include(p => p.User)
            .Where(p => p.UserId == user.Id)
            .ToListAsync();

        var productDtos = _mapper.Map<List<ProductDto>>(products);
        return View(productDtos);
    }
}