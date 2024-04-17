using System.Security.Claims;
using eTicaretDAL;
using eTicaretEL.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.Controllers;

[AllowAnonymous]
public class RegisterController : Controller
{
    private readonly DataContext _dataContext;

    public RegisterController(DataContext dataContext)
    {
        _dataContext = dataContext;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(eTicaretEL.Entities.User user)
    {
        if (ModelState.IsValid)
        {
            
            if (await _dataContext.User.AnyAsync(x => x.Email == user.Email))
            {
                ModelState.AddModelError("Email", "Bu e-posta adresi zaten kullanılıyor.");
                return View(user);
            }
          
            if (await _dataContext.User.AnyAsync(x => x.Name == user.Name))
            {
                ModelState.AddModelError("Kullanıcı Adı", "Bu kullanıcı adı zaten kullanılıyor.");
                return View(user);
            }

            user.UserRole = Role.User; // Default olarak kullanıcı rolü atanabilir
            user.RecordDate = DateTime.Now; // Kayıt tarihi ekleniyor

            _dataContext.User.Add(user);
            await _dataContext.SaveChangesAsync();
            

            return RedirectToAction("Index", "Home");
        }

        return View(user);
    }
}
