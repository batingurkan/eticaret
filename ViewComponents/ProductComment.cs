using AutoMapper;
using eTicaretDAL;
using eTicaretEL.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.ViewComponents;

public class ProductComment:ViewComponent
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public ProductComment(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }

    public async Task<IViewComponentResult> InvokeAsync(int id)
    {
        var comments = await _dataContext.Comment
            .Include(p => p.Product)
            .Where(x => x.ProductId == id)
            .ToListAsync();

        var commentDtos = _mapper.Map<List<CommentDto>>(comments);
        return View(commentDtos);
    }
}