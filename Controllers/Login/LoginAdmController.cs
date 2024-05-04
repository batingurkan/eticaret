using System.Security.Claims;
using eTicaretDAL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace eTicaret.Controllers.Login
{
    [AllowAnonymous]
    public class LoginAdmController : Controller
    {
        private readonly DataContext _dataContext;

        public LoginAdmController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(eTicaretEL.Entities.Admin admin)
        {
            var bilgiler = await _dataContext.Admin.FirstOrDefaultAsync(x => x.Name == admin.Name && x.Password == admin.Password);
            if (bilgiler != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, bilgiler.Name),
                    new Claim(ClaimTypes.NameIdentifier, bilgiler.Id.ToString())
                };

                var useridentity = new ClaimsIdentity(claims, "Login");
                ClaimsPrincipal principal = new ClaimsPrincipal(useridentity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "AdminUser");
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "LoginAdm");
        }
    }
}