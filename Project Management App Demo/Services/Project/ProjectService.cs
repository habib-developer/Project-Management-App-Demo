using Project_Management_App_Demo.Data;
using Project_Management_App_Demo.Models.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project_Management_App_Demo.Services.Project
{
    public class ProjectService:IProjectService
    {
        private readonly ApplicationDbContext _context;

        public ProjectService(ApplicationDbContext context)
        {
            this._context = context;
        }
        public IQueryable<ProjectDM> GetAllProjects()
        {
            var projects = from project in _context.Projects
                           select new ProjectDM()
                           {
                               ProjectID=project.ProjectID,
                               CreatedAt=project.CreatedAt,
                               Description=project.Description,
                               Name=project.Name,
                               UpdatedAt=project.UpdatedAt,
                               CreatedBy = _context.Users.Single(e => e.Id == project.CreatedBy).UserName,
                               UpdatedBy = _context.Users.Single(e => e.Id == project.UpdatedBy).UserName,
                           };
            return projects.AsQueryable();
        }
        public async Task<ProjectDM> GetById(int id)
        {
            return await _context.Projects.FindAsync(id);
        }
        public IQueryable<UserDM> GetProjectUsersById(int id)
        {
            var users = from user in _context.EmployeeUsers
                        join projectUser in _context.ProjectUsers on user.UserID equals projectUser.UserID
                        where projectUser.ProjectID == id
                        select new UserDM()
                        {
                            UserID = user.UserID,
                            Name = user.Name,
                            HoursPerWeek = user.HoursPerWeek,
                            RatePerHour = user.RatePerHour,
                            CreatedAt = user.CreatedAt,
                            UpdatedAt = user.UpdatedAt,
                            CreatedBy = _context.Users.Single(e => e.Id == user.CreatedBy).UserName,
                            UpdatedBy = _context.Users.Single(e => e.Id == user.UpdatedBy).UserName,
                        };
            return users.AsQueryable();
        }
        public async Task<ProjectDM> Create(ProjectDM entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            await _context.Projects.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<ProjectDM> Update(ProjectDM entity)
        {
            if (entity == null)
                throw new ArgumentNullException();
            _context.Projects.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task Delete(ProjectDM entity)
        {
            if (entity == null)
                throw new ArgumentNullException();
            _context.Projects.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public bool IsProjectExist(int id)
        {
            return _context.Projects.Any(e => e.ProjectID == id);
        }
        public IQueryable<UserDM> GetAvailableUsers(int projectId)
        {
            var users = from user in _context.EmployeeUsers
                        where !_context.ProjectUsers.Any(e => e.ProjectID == projectId && e.UserID == user.UserID)
                        select user;
            return users;
        }
        public async Task<ProjectUserDM> AddUser(ProjectUserDM entity)
        {
            if (entity == null)
                throw new ArgumentNullException();
            await _context.ProjectUsers.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<ProjectUserDM> GetProjectUserById(int id)
        {
            return await _context.ProjectUsers.FindAsync(id);
        }
        public ProjectUserDM GetProjectUser(int projectId,int userId)
        {
            return _context.ProjectUsers.FirstOrDefault(e=>e.ProjectID==projectId && e.UserID==userId);
        }

        public async Task RemoveUser(ProjectUserDM projectUser)
        {
            _context.ProjectUsers.Remove(projectUser);
            await _context.SaveChangesAsync();
        }
    }
}
