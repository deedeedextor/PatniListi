namespace PatniListi.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using PatniListi.Data.Models;

    public class CarUsersSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.CarUsers.Any())
            {
                return;
            }

            var companyIdOne = dbContext.Companies
                .Where(c => c.Name == "Авангард ЕООД")
                .Select(c => c.Id)
                .FirstOrDefault();

            var users = dbContext.ApplicationUsers
                .Where(u => u.CompanyId == companyIdOne)
                .Select(c => c.Id)
                .ToList();

            var cars = dbContext.Cars
                .Where(c => c.CompanyId == companyIdOne)
                .Select(c => c.Id)
                .ToList();

            for (int i = 0; i < cars.Count; i++)
            {
                await dbContext.CarUsers.AddAsync(new CarUser { CarId = cars[i], UserId = users[i] });
            }

            var companyIdTwo = dbContext.Companies
                .Where(c => c.Name == "ЕT Саламандър")
                .Select(c => c.Id)
                .FirstOrDefault();

            var usersTwo = dbContext.ApplicationUsers
                .Where(u => u.CompanyId == companyIdTwo)
                .Select(c => c.Id)
                .ToList();

            var carsTwo = dbContext.Cars
                .Where(c => c.CompanyId == companyIdTwo)
                .Select(c => c.Id)
                .ToList();

            for (int i = 0; i < carsTwo.Count; i++)
            {
                await dbContext.CarUsers.AddAsync(new CarUser { CarId = carsTwo[i], UserId = usersTwo[i] });
            }
        }
    }
}
