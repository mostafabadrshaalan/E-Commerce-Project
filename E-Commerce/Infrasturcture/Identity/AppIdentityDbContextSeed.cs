using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrasturcture.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUser(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser()
                {
                    DisplayName="Mostafa",
                    Email="mostafashaalan@gmail.com",
                    UserName= "mostafashaalan",
                    Address = new Address()
                    {
                        FirstName="Mostafa",
                        LastName="Shaalan",
                        City="Cairo",
                        State="Cairo",
                        Street="10 s",
                        ZipCode="12345"
                    }
                };
                await userManager.CreateAsync(user,"Password123!");
            }
        }
    }
}
