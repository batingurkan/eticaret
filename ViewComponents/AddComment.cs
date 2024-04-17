using eTicaretDAL;
using eTicaretEL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace eTicaret.ViewComponents;

public class AddComment:ViewComponent
{
    private readonly DataContext _dataContext;

    public AddComment(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    { 
        var commentModel = new Comment(); // Burada bir Comment nesnesi oluşturun
        return View(commentModel);
    }
    

}