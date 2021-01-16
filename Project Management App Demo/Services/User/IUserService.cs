using Project_Management_App_Demo.Models.Domain_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Management_App_Demo.Services.User
{
    public interface IUserService
    {
        IQueryable<UserDM> GetAllUsers();
        Task<UserDM> Create(UserDM entity);
        Task<UserDM> Update(UserDM entity);
        Task<UserDM> GetById(int id);
        bool IsUserExist(int id);
        Task Delete(UserDM entity);
    }
}
