using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServicesCourse.Models;

namespace ServicesCourse.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HistoriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HistoriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Histories
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.History.Include(h => h.Service).Include(h => h.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Histories/Details/5
        public async Task<IActionResult> Details(string login, DateTime accessTime)
        {

            var history = await _context.History
                .Include(h => h.Service)
                .Include(h => h.User)
                .FirstOrDefaultAsync(m => m.Login == login &&
                    m.AccessTime.Year == accessTime.Year &&
                    m.AccessTime.Month == accessTime.Month &&
                    m.AccessTime.Day == accessTime.Day &&
                    m.AccessTime.Hour == accessTime.Hour &&
                    m.AccessTime.Minute == accessTime.Minute &&
                    m.AccessTime.Second == accessTime.Second
                );

            if (history == null)
            {
                return NotFound();
            }

            return View(history);
        }
    }
}
