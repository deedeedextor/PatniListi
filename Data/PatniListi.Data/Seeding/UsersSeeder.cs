namespace PatniListi.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using PatniListi.Common;
    using PatniListi.Data.Models;

    public class UsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            if (dbContext.ApplicationUsers.Any())
            {
                return;
            }

            var companyIdOne = dbContext.Companies
                .Where(c => c.Name == "Авангард ЕООД")
                .Select(c => c.Id)
                .FirstOrDefault();

            var userAdminOne = new ApplicationUser
            {
                UserName = "Surya",
                Email = "surya@gmail.bg",
                CompanyId = companyIdOne,
                FullName = "Диди Димитрова",
                LastLoggingDate = DateTime.UtcNow,
            };

            var passwordAdmin = "admin6";

            var result = await userManager.CreateAsync(userAdminOne, passwordAdmin);

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(userAdminOne, GlobalConstants.AdministratorRoleName);
            }

            var regularUsersOne = new List<ApplicationUser>()
            {
                new ApplicationUser { UserName = "mars13", Email = "mars@gmail.com", CompanyId = companyIdOne, FullName = "Емилия Петрова", LastLoggingDate = DateTime.UtcNow },
                new ApplicationUser { UserName = "rima32", Email = "petrov.12@gmail.com", CompanyId = companyIdOne, FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow },
                new ApplicationUser { UserName = "Pipe", Email = "p.Peshev@abv.com", CompanyId = companyIdOne, FullName = "Павел Пешев", LastLoggingDate = DateTime.UtcNow },
                new ApplicationUser { UserName = "MariQ", Email = "maria_@gmail.com", CompanyId = companyIdOne, FullName = "Мария Илиева", LastLoggingDate = DateTime.UtcNow },
                new ApplicationUser { UserName = "IvaN", Email = "georgiev.Ivan@gmail.com", CompanyId = companyIdOne, FullName = "Иван Георгиев", LastLoggingDate = DateTime.UtcNow },
                new ApplicationUser { UserName = "Virus56", Email = "s.stoqnov@gmail.com", CompanyId = companyIdOne, FullName = "Станислав Стоянов", LastLoggingDate = DateTime.UtcNow },
                new ApplicationUser { UserName = "MiroSlav", Email = "miro@abv.com", CompanyId = companyIdOne, FullName = "Мирослав Петров", LastLoggingDate = DateTime.UtcNow },
                new ApplicationUser { UserName = "street5", Email = "street@gmail.com", CompanyId = companyIdOne, FullName = "Димо Димов", LastLoggingDate = DateTime.UtcNow },
                new ApplicationUser { UserName = "turtle", Email = "turtle@gmail.com", CompanyId = companyIdOne, FullName = "Симеон Вълков", LastLoggingDate = DateTime.UtcNow },
                new ApplicationUser { UserName = "stone", Email = "stone@abv.com", CompanyId = companyIdOne, FullName = "Стоян Стоянов", LastLoggingDate = DateTime.UtcNow },
                new ApplicationUser { UserName = "mars43", Email = "mars43@gmail.com", CompanyId = companyIdOne, FullName = "Елени Караиванова", LastLoggingDate = DateTime.UtcNow },
                new ApplicationUser { UserName = "mars22", Email = "mars22@gmail.com", CompanyId = companyIdOne, FullName = "Сис Христова", LastLoggingDate = DateTime.UtcNow },
            };

            for (int i = 0; i < regularUsersOne.Count; i++)
            {
                var password = $"regular{i}";

                var currentResult = await userManager.CreateAsync(regularUsersOne[i], password);

                if (currentResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(regularUsersOne[i], GlobalConstants.UserRoleName);
                }
            }

            var companyIdTwo = dbContext.Companies
                .Where(c => c.Name == "ЕT Саламандър")
                .Select(c => c.Id)
                .FirstOrDefault();

            var userAdminTwo = new ApplicationUser
            {
                UserName = "Phantom",
                Email = "phantom@abv.bg",
                CompanyId = companyIdTwo,
                FullName = "Дидо Димитров",
                LastLoggingDate = DateTime.UtcNow,
            };

            var passwordAdminTwo = "admin1";

            var resultTwo = await userManager.CreateAsync(userAdminTwo, passwordAdminTwo);

            if (resultTwo.Succeeded)
            {
                await userManager.AddToRoleAsync(userAdminTwo, GlobalConstants.AdministratorRoleName);
            }

            var regularUsersTwo = new List<ApplicationUser>()
            {
                new ApplicationUser { UserName = "mars130", Email = "mars0@gmail.com", CompanyId = companyIdTwo, FullName = "Емилия Илиева", LastLoggingDate = DateTime.UtcNow },
                new ApplicationUser { UserName = "rima320", Email = "petrov.120@gmail.com", CompanyId = companyIdTwo, FullName = "Петър Илиев", LastLoggingDate = DateTime.UtcNow },
            };

            for (int i = 0; i < regularUsersTwo.Count; i++)
            {
                var password = $"regular{i}";

                var currentResult = await userManager.CreateAsync(regularUsersTwo[i], password);

                if (currentResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(regularUsersTwo[i], GlobalConstants.UserRoleName);
                }
            }
        }
    }
}
