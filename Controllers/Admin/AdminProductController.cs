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
        
        List<SelectListItem> valuesize = (from x in _dataContext.Increases.ToList()
            select new SelectListItem
            {
                Text = x.Size,
                Value = x.Id.ToString()
            }).ToList();


        ViewBag.vlb = valuebrand;
        ViewBag.vlc = valuecategory;
        ViewBag.vls = valuesize;
    }

    public async Task<IActionResult> Index(int page = 1)
    {
        var list = await _dataContext.Product.ToListAsync();
        
        int pageSize = 9; 

        var paginatedProducts = list
            .OrderBy(s => s.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.CurrentPage = page; 
        
        ViewBag.v1 = list.Count();
        return View(paginatedProducts);
    }


    public async Task<IActionResult> UserProduct(int id,int page = 1)
    {
        var list = await _dataContext.Product.Where(x => x.UserId == id).ToListAsync();
        int pageSize = 9; 

        var paginatedProducts = list
            .OrderBy(s => s.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        ViewBag.v1 = list.Count();
        ViewBag.CurrentPage = page; 
        return View("Index", paginatedProducts);
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
        product.IncreasesId = p.IncreasesId;
        product.UserId = product.UserId;
        product.IsApproved = p.IsApproved;
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
    
    public async Task<IActionResult> IsApprovedProduct(int page = 1)
    {
        var list = await _dataContext.Product.Where(x=>x.IsApproved==false).ToListAsync();
        
        int pageSize = 9; 

        var paginatedProducts = list
            .OrderBy(s => s.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.CurrentPage = page; 
        
        ViewBag.v1 = list.Count();
        return View(paginatedProducts);
    }
}