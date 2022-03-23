using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServicesCourse.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace ServicesCourse.Controllers
{
    public class HomeController : Controller
    {
        ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Admin"))
                {
                    return Redirect("Admin");
                }
                return Redirect("User");
            }
            return View();
        }


        [HttpGet("signup")]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUpValidateAsync(string login, string password, string passwordVer)
        {
            var user = _context.User.FirstOrDefault(s => s.Login == login);   
            if (user == null)
            {
                if (password != passwordVer)
                {
                    TempData["Error"] = "Пароли не совпадают";
                    return View("signup");
                }

                await _context.User.AddAsync(new User()
                {
                    Login = login,
                    Password = password,
                    UserTypeId = 2,
                    ActivityStatus = true
                });

                _context.SaveChanges();

                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Role, "User"));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, login));
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                Thread.CurrentPrincipal = claimsPrincipal;
                await HttpContext.SignInAsync(claimsPrincipal);

                return Redirect("User");
            }
            TempData["Error"] = "Логин занят";
            return View("signup");
        }


        [HttpGet("login")]
        public IActionResult Login()
        {
            return View();
        }


        [HttpGet("denied")]
        public IActionResult AccessDenied()
        {
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Validate(string login, string password)
        {
            var user = _context.User
                .Include(u => u.UserType)
                .FirstOrDefault(s => s.Login == login); 
            if (user != null)
            {
                if (password == user.Password)
                {   if (!user.ActivityStatus) {
                        TempData["Error"] = "Аккаунт не активен";
                        return View("login");
                    }
                    var typename = user.UserType.UserTypeName;
                    if (typename == UserTypes.Администратор)
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, login));
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        Thread.CurrentPrincipal = claimsPrincipal;
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return Redirect("admin");
                    }
                    if (typename == UserTypes.Пользователь)
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Role, "User"));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, login));
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        Thread.CurrentPrincipal = claimsPrincipal;
                        await HttpContext.SignInAsync(claimsPrincipal);
                        return Redirect("user");
                    }

                }
            }
            TempData["Error"] = "Неверный логин или пароль";
            return View("login");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
