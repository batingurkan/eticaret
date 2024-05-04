using AutoMapper;
using eTicaretBL.Abstract;
using eTicaretDAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.Controllers.Admin;


public class AdminUserController : Controller
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly IUserServices _userService;

    public AdminUserController(DataContext dataContext, IMapper mapper, IUserServices userService)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _userService = userService;
    }

   
    public async Task<IActionResult>  Index()
    {
        var list = _userService.GetList();
        ViewBag.v1 = list.Count();
        return View(list);
    }
 
    [HttpGet]
    public async Task<IActionResult> UserEdit(int id)
    {
        var list =await _dataContext.User.Where(x=>x.Id == id).FirstOrDefaultAsync();
        return View(list);
    }
    [HttpPost]
    public async Task<IActionResult> UserEdit(eTicaretEL.Entities.User p)
    {
        var user = _dataContext.User.Where(x => x.Id == p.Id).FirstOrDefault();
        user.Name = p.Name;
        user.Email = p.Email;
        user.Password = p.Password;
        user.UserRole = p.UserRole;
        user.RecordDate = user.RecordDate;
        user.Id = user.Id;
        _dataContext.User.Update(user);
        await _dataContext.SaveChangesAsync();

        return RedirectToAction("Index", "AdminProduct");
    }
    
}