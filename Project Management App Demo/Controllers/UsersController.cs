using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Project_Management_App_Demo.Data;
using Project_Management_App_Demo.Models.Domain_Models;
using Project_Management_App_Demo.Services.User;

namespace Project_Management_App_Demo.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            this._userService = userService;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            return View(await _userService.GetAllUsers().ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDM = await _userService.GetById(id.Value);
            if (userDM == null)
            {
                return NotFound();
            }

            return View(userDM);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectUserID,Name,HoursPerWeek,RatePerHour,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt")] UserDM userDM)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                userDM.CreatedAt = DateTime.UtcNow;
                userDM.CreatedBy = userId;
                await _userService.Create(userDM);
                return RedirectToAction(nameof(Index));
            }
            return View(userDM);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDM = await _userService.GetById(id.Value);
            if (userDM == null)
            {
                return NotFound();
            }
            return View(userDM);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectUserID,Name,HoursPerWeek,RatePerHour,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt")] UserDM userDM)
        {
            if (id != userDM.UserID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    userDM.UpdatedAt = DateTime.UtcNow;
                    userDM.UpdatedBy = userId;
                    await _userService.Update(userDM);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_userService.IsUserExist(userDM.UserID))
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
            return View(userDM);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userDM = await _userService.GetById(id.Value);
            if (userDM == null)
            {
                return NotFound();
            }

            return View(userDM);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userDM = await _userService.GetById(id);
            await _userService.Delete(userDM);
            return RedirectToAction(nameof(Index));
        }

    }
}
