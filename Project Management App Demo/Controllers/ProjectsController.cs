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
using Project_Management_App_Demo.Services.Project;
using Project_Management_App_Demo.Services.User;

namespace Project_Management_App_Demo.Controllers
{
    [Authorize]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IUserService _userService;

        public ProjectsController(IProjectService projectService,IUserService userService)
        {
            this._projectService = projectService;
            this._userService = userService;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _projectService.GetAllProjects().ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectDM = await _projectService.GetById(id.Value);
            if (projectDM == null)
            {
                return NotFound();
            }

            return View(projectDM);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectID,Name,Description,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt")] ProjectDM projectDM)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                projectDM.CreatedBy = userId;
                projectDM.CreatedAt = DateTime.UtcNow;
                await _projectService.Create(projectDM);
                return RedirectToAction(nameof(Index));
            }
            return View(projectDM);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectDM = await _projectService.GetById(id.Value);
            if (projectDM == null)
            {
                return NotFound();
            }
            return View(projectDM);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectID,Name,Description,CreatedBy,CreatedAt,UpdatedBy,UpdatedAt")] ProjectDM projectDM)
        {
            if (id != projectDM.ProjectID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                    projectDM.UpdatedAt = DateTime.UtcNow;
                    projectDM.UpdatedBy = userId;
                    await _projectService.Update(projectDM);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_projectService.IsProjectExist(projectDM.ProjectID))
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
            return View(projectDM);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var projectDM = await _projectService.GetById(id.Value);
            if (projectDM == null)
            {
                return NotFound();
            }

            return View(projectDM);
        }
        public async Task<ActionResult> Users(int id)
        {
            ViewBag.ProjectId = id;
            var project = await _projectService.GetById(id);
            ViewBag.ProjectName = project.Name;
            var users = _projectService.GetProjectUsersById(id);
            return View(await users.ToListAsync());
        }
        public async Task<ActionResult> AddUser(int projectId)
        {
            var users = await _projectService.GetAvailableUsers(projectId).ToListAsync();
            ViewBag.UserID = new SelectList(users, "UserID", "Name");
            return View(new ProjectUserDM() { ProjectID=projectId});
        }
        [HttpPost]
        public async Task<ActionResult> AddUser(ProjectUserDM projectUser)
        {
            if (ModelState.IsValid)
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                projectUser.CreatedBy = userId;
                projectUser.CreatedAt = DateTime.UtcNow;
                await _projectService.AddUser(projectUser);
                return RedirectToAction("Users", new { id=projectUser.ProjectID });
            }
            return View(projectUser);
        }
        public ActionResult RemoveUser(int projectId,int userId)
        {
            var projectUser = _projectService.GetProjectUser(projectId, userId);
            if (projectUser == null)
                return BadRequest();
            return View(projectUser);
        }
        [HttpPost]
        public async Task<ActionResult> RemoveUserConfirmed(ProjectUserDM projectUser)
        {
            var projectId = projectUser.ProjectID;
            await _projectService.RemoveUser(projectUser);
            return RedirectToAction("Users", new { id=projectId });
        }
        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var projectDM = await _projectService.GetById(id);
            await _projectService.Delete(projectDM);
            return RedirectToAction(nameof(Index));
        }

    }
}
