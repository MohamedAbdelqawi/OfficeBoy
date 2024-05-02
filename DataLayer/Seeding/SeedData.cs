using DataLayer.Context;
using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DataLayer.Seeding
{
    public class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Employee>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();


                context.Database.EnsureCreated();
                await SeedRoles(roleManager);

                if (userManager.Users.Any(u => u.Email == "admin@gmail.com"))
                {
                    return;
                }
                else
                {
                    await SeedEmployee(userManager);
                }


                if (!context.OfficeLocations.Any())
                {
                    SeedOfficeLocations(context);
                }

            }
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] roleNames = { "Admin", "OfficeBoy" };

            foreach (var roleName in roleNames)
            {

                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {

                    var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!result.Succeeded)
                    {
                        throw new Exception($"Error creating role '{roleName}': {result.Errors.First().Description}");
                    }
                }
            }
        }

        private static async Task SeedEmployee(UserManager<Employee> userManager)
        {
            var employee = new Employee
            {
                UserName = "admin@gmail.com",
                Name = "admin@gmail.com",
                Email = "admin@gmail.com",
                PhoneNumber = "123456",
                OfficeLocation = "First floor",
                EmpId = "E001"
            };

            var existingUser = await userManager.FindByEmailAsync(employee.Email);
            if (existingUser == null)
            {
                var result = await userManager.CreateAsync(employee, "123456");

                if (!result.Succeeded)
                {
                    throw new Exception($"Error creating employee user: {result.Errors.First().Description}");
                }


                await userManager.AddToRoleAsync(employee, "Admin");
            }
        }

        private static void SeedOfficeLocations(ApplicationDbContext context)
        {
            context.OfficeLocations.AddRange(
                new OfficeLocation { Name = "First floor ", Address = " first office" },
                new OfficeLocation { Name = "First floor", Address = "second office" },
                new OfficeLocation { Name = "First floor", Address = "third  office" },
                new OfficeLocation { Name = "second floor ", Address = " first office" },
                new OfficeLocation { Name = "second floor", Address = "second office" },
                new OfficeLocation { Name = "second floor", Address = "third  office" }
            );

            context.SaveChanges();
        }
    }
}


