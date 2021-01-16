using Project_Management_App_Demo.Data;
using Project_Management_App_Demo.Models.Domain_Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Project_Management_App_Demo.Services.User
{
    public class UserService:IUserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            this._context = context;
        }
        public IQueryable<UserDM> GetAllUsers()
        {
            var users = from user in _context.EmployeeUsers
                       select new UserDM()
                       {
                           UserID=user.UserID,
                           Name=user.Name,
                           HoursPerWeek=user.HoursPerWeek,
                           RatePerHour=user.RatePerHour,
                           CreatedAt=user.CreatedAt,
                           UpdatedAt=user.UpdatedAt,
                           CreatedBy=_context.Users.Single(e=>e.Id==user.CreatedBy).UserName,
                           UpdatedBy=_context.Users.Single(e=>e.Id==user.UpdatedBy).UserName,
                       };
            return users.AsQueryable();
        }
        public async Task<UserDM> GetById(int id)
        {
            return await _context.EmployeeUsers.FindAsync(id);
        }
        public async Task<UserDM> Create(UserDM entity)
        {
            if (entity == null)
                throw new ArgumentNullException("entity");
            await _context.EmployeeUsers.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task<UserDM> Update(UserDM entity)
        {
            if (entity == null)
                throw new ArgumentNullException();
            _context.EmployeeUsers.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
        public async Task Delete(UserDM entity)
        {
            if (entity == null)
                throw new ArgumentNullException();
            _context.EmployeeUsers.Remove(entity);
            await _context.SaveChangesAsync();
        }
        public bool IsUserExist(int id)
        {
            return _context.EmployeeUsers.Any(e => e.UserID == id);
        }
    }
}
