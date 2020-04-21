namespace PatniListi.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PatniListi.Data.Models;

    public class InvoicesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.Invoices.Any())
            {
                return;
            }

            var companyIdOne = dbContext.Companies
                .Where(c => c.Name == "Авангард ЕООД")
                .Select(c => c.Id)
                .FirstOrDefault();

            var cars = dbContext.Cars
                .Where(c => c.CompanyId == companyIdOne)
                .Include(c => c.CarUsers)
                .ToList();

            var invoices = new List<Invoice>()
            {
                new Invoice { Number = "100023413", Date = DateTime.UtcNow, Location = "София", Price = 1.99m, Quantity = 10, TotalPrice = 1.99m * 10 },
                new Invoice { Number = "1100234123", Date = DateTime.UtcNow, Location = "Пловдив", Price = 1.98m, Quantity = 10, TotalPrice = 1.98m * 10 },
                new Invoice { Number = "1000634123", Date = DateTime.UtcNow, Location = "Стара Загора", Price = 2.03m, Quantity = 10, TotalPrice = 2.03m * 10 },
                new Invoice { Number = "1001632123", Date = DateTime.UtcNow, Location = "Хасково", Price = 2.13m, Quantity = 10, TotalPrice = 2.13m * 10 },
                new Invoice { Number = "1007634152", Date = DateTime.UtcNow, Location = "Бургас", Price = 2.00m, Quantity = 10, TotalPrice = 2.00m * 10 },
                new Invoice { Number = "1011637123", Date = DateTime.UtcNow, Location = "София", Price = 2.15m, Quantity = 10, TotalPrice = 2.15m * 10 },
                new Invoice { Number = "1000634001", Date = DateTime.UtcNow, Location = "Варна", Price = 2.09m, Quantity = 10, TotalPrice = 2.09m * 10 },
                new Invoice { Number = "1000634983", Date = DateTime.UtcNow, Location = "Пловдив", Price = 2.20m, Quantity = 10, TotalPrice = 2.20m * 10 },
                new Invoice { Number = "1200634823", Date = DateTime.UtcNow, Location = "Бургас", Price = 1.87m, Quantity = 10, TotalPrice = 1.87m * 10 },
                new Invoice { Number = "1000544123", Date = DateTime.UtcNow, Location = "Стара Загора", Price = 1.98m, Quantity = 10, TotalPrice = 1.98m * 10 },
                new Invoice { Number = "1033734135", Date = DateTime.UtcNow, Location = "София", Price = 2.01m, Quantity = 10, TotalPrice = 2.01m * 10 },
                new Invoice { Number = "1002634100", Date = DateTime.UtcNow, Location = "Пловдив", Price = 2.02m, Quantity = 10, TotalPrice = 2.02m * 10 },
                new Invoice { Number = "1002634199", Date = DateTime.UtcNow, Location = "София", Price = 2.04m, Quantity = 10, TotalPrice = 2.04m * 10 },
            };

            for (int i = 0; i < cars.Count; i++)
            {
                invoices[i].CarId = cars[i].Id;
                invoices[i].UserId = cars[i].CarUsers.Select(cu => cu.UserId).FirstOrDefault();
                invoices[i].CreatedBy = cars[i].CarUsers.Select(cu => cu.ApplicationUser.FullName).FirstOrDefault();
                invoices[i].FuelType = cars[i].FuelType.ToString();
                invoices[i].CurrentLiters = cars[i].InitialFuel;

                await dbContext.Invoices.AddAsync(invoices[i]);
            }

            var companyIdTwo = dbContext.Companies
               .Where(c => c.Name == "ЕT Саламандър")
               .Select(c => c.Id)
               .FirstOrDefault();

            var carsTwo = dbContext.Cars
                .Where(c => c.CompanyId == companyIdTwo)
                .Include(c => c.CarUsers)
                .ToList();

            var invoicesTwo = new List<Invoice>()
            {
                new Invoice { Number = "100023410", Date = DateTime.UtcNow, Location = "София", Price = 2.05m, Quantity = 15, TotalPrice = 2.05m * 15 },
                new Invoice { Number = "1100234123", Date = DateTime.UtcNow, Location = "Велико Търново", Price = 1.97m, Quantity = 20, TotalPrice = 1.97m * 20 },
                new Invoice { Number = "1100234583", Date = DateTime.UtcNow, Location = "Пловдив", Price = 1.90m, Quantity = 10, TotalPrice = 1.90m * 10 },
            };

            for (int i = 0; i < carsTwo.Count; i++)
            {
                invoicesTwo[i].CarId = carsTwo[i].Id;
                invoicesTwo[i].UserId = carsTwo[i].CarUsers.Select(cu => cu.UserId).FirstOrDefault();
                invoicesTwo[i].CreatedBy = carsTwo[i].CarUsers.Select(cu => cu.ApplicationUser.FullName).FirstOrDefault();
                invoicesTwo[i].FuelType = carsTwo[i].FuelType.ToString();
                invoicesTwo[i].CurrentLiters = carsTwo[i].InitialFuel;

                await dbContext.Invoices.AddAsync(invoicesTwo[i]);
            }
        }
    }
}
