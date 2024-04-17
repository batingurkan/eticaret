using AutoMapper;
using eTicaretDAL;
using eTicaretEL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.Controllers.Admin;


public class AdminProductController : Controller
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public AdminProductController(DataContext dataContext, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
    }


    private void PopulateDropdowns()
    {
        List<SelectListItem> valuecategory = (from x in _dataContext.Category.ToList()
            select new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();

        List<SelectListItem> valuebrand = (from x in _dataContext.Brand.ToList()
            select new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            }).ToList();


        ViewBag.vlb = valuebrand;
        ViewBag.vlc = valuecategory;
    }

    public async Task<IActionResult> Index()
    {
        var list = await _dataContext.Product.ToListAsync();
        return View(list);
    }


    public async Task<IActionResult> UserProduct(int id)
    {
        var list = await _dataContext.Product.Where(x => x.UserId == id).ToListAsync();
        return View("Index", list);
    }

    [HttpGet]
    public async Task<IActionResult> UserProductEdit(int id)
    {
        PopulateDropdowns();
        var list = await _dataContext.Product.Where(x => x.Id == id).FirstOrDefaultAsync();
        return View(list);
    }

    [HttpPost]
    public async Task<IActionResult> UserProductEdit(Product p)
    {
        var product = _dataContext.Product.Where(x => x.Id == p.Id).FirstOrDefault();
        product.Name = p.Name;
        product.Description = p.Description;
        product.Price = p.Price;
        product.BrandId = p.BrandId;
        product.Model = p.Model;
        product.CategoryId = p.CategoryId;
        product.UserId = product.UserId;
        _dataContext.Product.Update(product);
        await _dataContext.SaveChangesAsync();

        return RedirectToAction("Index", "AdminProduct");
    }

    public async Task<IActionResult> UserProductDelete(int id)
    {
        var delete = _dataContext.Product.FirstOrDefault(x => x.Id == id);
        
        var cartItem = _dataContext.CartItem.Where(a => a.ProductId == id);
        _dataContext.CartItem.RemoveRange(cartItem);
        
        
        _dataContext.Product.Remove(delete);
        await _dataContext.SaveChangesAsync();

        return RedirectToAction("Index", "AdminProduct");
    }
}