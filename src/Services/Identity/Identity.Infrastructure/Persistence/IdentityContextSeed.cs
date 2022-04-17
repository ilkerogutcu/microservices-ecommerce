using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Identity.Domain.Entities;
using Identity.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Identity.Infrastructure.Persistence
{
    public class IdentityContextSeed
    {
        public static async Task SeedAsync(IdentityContext identityContext, IServiceProvider services)
        {
            var user = new User
            {
                Id = Guid.NewGuid().ToString().ToUpper(),
                FirstName = "ilker",
                LastName = "öğütcü",
                Email = "ilkerogtc@gmail.com",
                NormalizedEmail = "ILKEROGTC@GMAIL.COM",
                PhoneNumber = "+905557778899",
                UserName = "ilkerogtc@gmail.com",
                NormalizedUserName = "ILKEROGTC@GMAIL.COM",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D"),
                BirthDate = new DateTime(2000, 02, 03),
                TwoFactorEnabled = false,
                CreatedDate = DateTime.Now
            };
            await SeedRolesAsync(identityContext, services);
            await SeedCitiesOfDistricts(identityContext, user);
            await SeedUsers(identityContext, user, services);
            identityContext.SaveChanges();
        }

        private static async Task SeedUsers(IdentityContext identityContext, User user, IServiceProvider services)
        {
            if (!identityContext.Users.Any())
            {
                var userManager = services.GetService<UserManager<User>>();
                await userManager.CreateAsync(user, "Jrypb3;<(8atpHyZ");
                await userManager.AddToRoleAsync(user, Role.Administrator.ToString());
                await userManager.AddToRoleAsync(user, Role.Buyer.ToString());
            }
        }

        private static async Task SeedRolesAsync(IdentityContext identityContext, IServiceProvider services)
        {
            if (!identityContext.Roles.Any())
            {
                var roles = new string[] {"Administrator", "Editor", "Buyer"};
                var roleManager = services.GetService<RoleManager<IdentityRole>>();
                foreach (string role in roles)
                {
                    await roleManager.CreateAsync(new IdentityRole()
                    {
                        Name = role,
                        NormalizedName = role.ToUpper()
                    });
                }
            }
        }

        private static async Task SeedCitiesOfDistricts(IdentityContext identityContext, User user)
        {
            if (!identityContext.Cities.Any() && !identityContext.Districts.Any())
            {
                var currentDir = Path.GetDirectoryName(Environment.CurrentDirectory);
                Console.WriteLine(currentDir);
                var citiesOfDistrictsList =
                    JsonConvert.DeserializeObject<List<CitiesOfDistrict>>(File.ReadAllText(
                        $@"{currentDir}/Identity.Infrastructure/Persistence/SeedHelpers/CitiesOfDistricts.json"));
                if (citiesOfDistrictsList is not null)
                {
                    foreach (var citiesOfDistricts in citiesOfDistrictsList)
                    {
                        var city = await identityContext.Cities.AddAsync(new City(citiesOfDistricts.City, user.Id));
                        foreach (var district in citiesOfDistricts.Districts)
                        {
                            await identityContext.Districts.AddAsync(new District(city.Entity, district, user.Id));
                        }
                    }
                }

                await identityContext.SaveChangesAsync();
            }
        }

        private class CitiesOfDistrict
        {
            public string City { get; set; }
            public int PlateNumber { get; set; }
            public string[] Districts { get; set; }
        }
    }
}