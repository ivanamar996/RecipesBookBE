using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RecepiesBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecepiesBook.Data
{
    public class SeedDatabase
    {
        public static void Initialize(IServiceProvider serviceProvider) 
        {
            var context = serviceProvider.GetRequiredService<RecepiesBookDbContext>();

            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            context.Database.EnsureCreated();

            if (!context.Users.Any())
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = "ivana123@gmail.com",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = "Ivana",
                };

                userManager.CreateAsync(user, "Ivana123!");

            }
        }
    }
}
