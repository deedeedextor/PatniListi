namespace PatniListi.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PatniListi.Data.Models;

    public class CompaniesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Companies.Any())
            {
                return;
            }

            var companies = new List<Company>()
            {
                new Company { Name = "Авангард ЕООД" },
                new Company { Name = "ЕT Саламандър" },
            };

            await dbContext.Companies.AddRangeAsync(companies);
        }
    }
}
