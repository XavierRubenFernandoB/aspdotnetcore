using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace NetCoreProj.Models
{
    //ApplicationUser is added for migration to be created successfully. Since it will refer to IdentityUser instead and create empty files during migration
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        {
                
        }

        public DbSet<MEmployee> EmployeesDBSet { get; set; }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
        //    base.OnModelCreating(builder);
        //    //builder.Seed();
        //}
    }
}
