using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ServicesCourse.Controllers
{
    public class UserController : Controller
    {
        [Authorize(Roles="User")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
