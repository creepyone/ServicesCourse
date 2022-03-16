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

        //public HomeController()
        //{
        //    this.dataBaseRepository = dataBaseRepository;
        //}


        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("signup")]
        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost("signup")]
        public IActionResult SignUpValidate()
        {
            return View();
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
            var user = dataBaseRepository.GetUserByLogin(login);
            if (user != null)
            {
                if (password == user.Password)
                {
                    var userType = dataBaseRepository.GetUserType(user.UserTypeId);
                    if (userType == UserTypes.Администратор)
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, login));
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        TempData["Role"] = "Admin";
                        return Redirect("Admin/Index");
                    }
                    if (userType == UserTypes.Пользователь)
                    {
                        var claims = new List<Claim>();
                        claims.Add(new Claim(ClaimTypes.Role, "User"));
                        claims.Add(new Claim(ClaimTypes.NameIdentifier, login));
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                        await HttpContext.SignInAsync(claimsPrincipal);
                        TempData["Role"] = "User";
                        return Redirect("User/Index");
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
