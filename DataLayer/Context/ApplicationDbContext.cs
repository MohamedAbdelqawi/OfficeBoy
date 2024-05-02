using DataLayer.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Context
{
    public class ApplicationDbContext : IdentityDbContext<Employee>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Drink> Drinks { get; set; }
        public DbSet<OfficeLocation> OfficeLocations { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OfficeBoyShift> OfficeBoyShifts { get; set; }



    }
}
