using eTicaretDAL;
using Microsoft.AspNetCore.Mvc;

namespace eTicaret.Controllers.Admin;

public class StatisticsController : Controller
{
    private readonly DataContext _dataContext;

    public StatisticsController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public IActionResult Index()
    {
        ViewBag.v1 = _dataContext.User.Count();
        ViewBag.v2 = _dataContext.Product.Count();
        ViewBag.v3 = _dataContext.Order.Count();
        
        decimal toplamTutar = 0;
        foreach (var orderDetail in _dataContext.Order)
        {
            toplamTutar += orderDetail.TotalAmount;
        }
        ViewBag.v4 = toplamTutar;
        
        ViewBag.v5 = _dataContext.OrderDetail.Count();
        ViewBag.v1 = _dataContext.User.Count();
        return View();
    }
}