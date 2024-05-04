using AutoMapper;
using eTicaretDAL;
using eTicaretEL.Dtos;
using eTicaretEL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.ViewComponents;

public class CommentAdd:ViewComponent
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public CommentAdd(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }
    [HttpGet]
    public async Task<IViewComponentResult> InvokeAsync(int productId,int userId)
    {
        var comment = new Comment { ProductId = productId ,UserId = userId };
        return View(comment);
    }
}