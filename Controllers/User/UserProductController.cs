using AutoMapper;
using eTicaretDAL;
using eTicaretEL.AbstractValidator;
using eTicaretEL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace eTicaret.Controllers.User;

public class UserProductController : Controller
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;

    public UserProductController(DataContext dataContext, IMapper mapper)
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
    
    [HttpGet]
    public IActionResult ProductAdd()
    {
        
        PopulateDropdowns();
        return View();
    }
   [HttpPost]
public IActionResult ProductAdd(ProductAdd p)
{

    var userId = HttpContext.Session.GetInt32("userId");
    if (userId.HasValue && p != null)
    { 
        var product = new Product();
        if (p.Img != null && p.Img.Length > 0)
        {
            var extension = Path.GetExtension(p.Img.FileName);
            var newImageName = Guid.NewGuid() + extension;
            var location = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/electro-master/img/", newImageName);
            using (var stream = new FileStream(location, FileMode.Create))
            {
                p.Img.CopyTo(stream);
            }
            product.Img = newImageName;
        }
        product.Name = p.Name;
        product.Description = p.Description;
        product.Price = p.Price;
        product.BrandId = p.BrandId;
        product.Model = p.Model;
        product.CategoryId = p.CategoryId;
        product.UserId = userId.Value;
        
        var validator = new ProdcutValidator();
        var result = validator.Validate(product);
        
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            
            PopulateDropdowns();
            return View();
        }
        _dataContext.Product.Add(product);
        _dataContext.SaveChanges();
        
        return RedirectToAction("Index", "Home");
    }
    return BadRequest();
}

[HttpGet]
public IActionResult ProductEdit(int id)
{
    PopulateDropdowns();
    var list = _dataContext.Product.Where(x => x.Id == id).FirstOrDefault();
    return View(list);
}

[HttpPost]
public IActionResult ProductEdit(Product p)
{
    var userId = HttpContext.Session.GetInt32("userId");
    var product = _dataContext.Product.Where(x => x.Id == p.Id).FirstOrDefault();
    product.Name = p.Name;
    product.Description = p.Description;
    product.Price = p.Price;
    product.BrandId = p.BrandId;
    product.Model = p.Model;
    product.CategoryId = p.CategoryId;
    product.UserId = userId.Value;
    _dataContext.Product.Update(product);
    _dataContext.SaveChanges();

    return RedirectToAction("Index", "Home");
}


public IActionResult ProductDelete(int id)
{
    var delete = _dataContext.Product.FirstOrDefault(x => x.Id == id);
            
    var cartItem = _dataContext.CartItem.Where(a => a.ProductId == id);
    _dataContext.CartItem.RemoveRange(cartItem);
    
    _dataContext.Product.Remove(delete);
    _dataContext.SaveChanges();
    
    return RedirectToAction("Index","Home");
}
}


