using System.Security.Claims;
using eTicaretDAL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.Controllers.Login;
[AllowAnonymous]
public class LoginController : Controller
{
    private readonly DataContext _dataContext;

    public LoginController(DataContext dataContext)
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
        var bilgiler = await _dataContext.User.FirstOrDefaultAsync(x => x.Email == user.Email && x.Password == user.Password);
        if (bilgiler != null)
        {
            // Kullanıcı bilgilerini oturumda saklayın
            HttpContext.Session.SetString("usermail", bilgiler.Email);
            HttpContext.Session.SetInt32("userId", bilgiler.Id);


            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, bilgiler.Email),
                new Claim(ClaimTypes.NameIdentifier, bilgiler.Id.ToString())
            };

        
            var useridentity = new ClaimsIdentity(claims, "Login");
            ClaimsPrincipal principal = new ClaimsPrincipal(useridentity);
            await HttpContext.SignInAsync(principal);
            return RedirectToAction("Index","Home");
        }
       
        return View();
    }
    [HttpGet]

    public async Task<IActionResult> Logout()
    {

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return RedirectToAction("Index", "Login");
    }
}