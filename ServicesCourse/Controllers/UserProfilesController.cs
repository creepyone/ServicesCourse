﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ServicesCourse.Models;

namespace ServicesCourse.Controllers
{
    public class UserProfilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly DataBaseRepository _dataBaseRepository;

        public UserProfilesController(ApplicationDbContext context)
        {
            _context = context;
            _dataBaseRepository = new DataBaseRepository(_context);

        }

        // GET: UserProfiles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.UserProfile.Include(c => c.Sex).Include(c => c.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: UserProfiles/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserProfile
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Login == id);

            if (userProfile == null)
            {
                return NotFound();
            }

            return View(userProfile);
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

        // POST: UserProfiles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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