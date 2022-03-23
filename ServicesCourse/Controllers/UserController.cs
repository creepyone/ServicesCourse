using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServicesCourse.Models;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ServicesCourse.Controllers
{
    [Authorize(Roles = "User")]
    public class UserController : Controller
    {

        ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewData["Services"] = _context.Service
                .Include(s => s.Subsection)
                .ThenInclude(s => s.Section)
                .Where(s => s.ActivityStatus)
                .ToListAsync().Result;

            return View();
        }

        [HttpGet]
        public IActionResult Service(int? id)
        {
            ViewData["ServiceInfo"] =  _context.Service
                .Include(s => s.Subsection)
                .ThenInclude(s => s.Section)
                .FirstOrDefaultAsync(s => s.Id == id).Result;


            return View();
        }

        [HttpPost]
        public IActionResult ServiceAccessed(int id)
        {
            _context.History.Add(new History()
            {
                Login = User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.NameIdentifier).Value,
                AccessTime = DateTime.Now,
                ServiceId = id
            });
            _context.SaveChanges();
            return Redirect("/User/Index");
        }


        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfile.FindAsync(id);
            if (userProfile == null)
            {
                return NotFound();
            }
            ViewData["SexId"] = new SelectList(_context.Sex, "Id", "SexName");
            return View(userProfile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Login,PhoneNumber,Surname,Name,Patronymic,SexId,BirthDate,Email")] UserProfile userProfile)
        {
            if (id != userProfile.Login)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userProfile);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserProfileExists(userProfile.Login))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Login"] = new SelectList(_context.User, "Login", "Login", userProfile.Login);
            return View(userProfile);
        }

        private bool UserProfileExists(string id)
        {
            return _context.UserProfile.Any(e => e.Login == id);
        }
    }
}
