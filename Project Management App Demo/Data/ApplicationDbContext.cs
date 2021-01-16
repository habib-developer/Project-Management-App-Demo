using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Project_Management_App_Demo.Models.Domain_Models;

namespace Project_Management_App_Demo.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Project_Management_App_Demo.Models.Domain_Models.ProjectDM> Projects { get; set; }
        public DbSet<Project_Management_App_Demo.Models.Domain_Models.ProjectUserDM> ProjectUsers { get; set; }
        public DbSet<Project_Management_App_Demo.Models.Domain_Models.UserDM> EmployeeUsers { get; set; }
    }
}
