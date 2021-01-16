using Project_Management_App_Demo.Models.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Management_App_Demo.Services.Project
{
    public interface IProjectService
    {
        IQueryable<ProjectDM> GetAllProjects();
        Task<ProjectDM> Create(ProjectDM entity);
        Task<ProjectDM> Update(ProjectDM entity);
        Task<ProjectDM> GetById(int id);
        IQueryable<UserDM> GetProjectUsersById(int id);
        bool IsProjectExist(int id);
        Task Delete(ProjectDM entity);
        Task<ProjectUserDM> AddUser(ProjectUserDM entity);
        Task RemoveUser(ProjectUserDM projectUser);
        ProjectUserDM GetProjectUser(int projectId, int userId);
        Task<ProjectUserDM> GetProjectUserById(int id);
        public IQueryable<UserDM> GetAvailableUsers(int projectId);


    }
}
