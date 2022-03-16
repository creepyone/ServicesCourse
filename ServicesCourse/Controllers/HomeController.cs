using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicesCourse.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ServicesCourse.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DataBaseRepository dataBaseRepository;

        public HomeController(ILogger<HomeController> logger, DataBaseRepository dataBaseRepository)
        {
            _logger = logger;
            this.dataBaseRepository = dataBaseRepository;
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
            var user = dataBaseRepository.GetUserByLogin(login).Result;
            if (user == null)
            {
                if (password != passwordVer)
                {
                    TempData["Error"] = "Пароли не совпадают";
                    return View("signup");
                }

                await dataBaseRepository.AddNewUser(login, password);

                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Role, "User"));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, login));
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
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
            var user = dataBaseRepository.GetUserByLogin(login).Result;
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
