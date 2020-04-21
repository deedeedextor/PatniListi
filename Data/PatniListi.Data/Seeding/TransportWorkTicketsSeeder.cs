namespace PatniListi.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PatniListi.Data.Models;

    public class TransportWorkTicketsSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (dbContext.TransportWorkTickets.Any())
            {
                return;
            }

            var companyIdOne = dbContext.Companies
                .Where(c => c.Name == "Авангард ЕООД")
                .Select(c => c.Id)
                .FirstOrDefault();

            var cars = dbContext.Cars
                .Where(c => c.CompanyId == companyIdOne)
                .Include(c => c.Invoices)
                .Include(c => c.CarUsers)
                .ToList();

            var routes = dbContext.Routes
                .ToList();

            for (int i = 0; i < cars.Count; i++)
            {
                var travelledDistance = routes[i].Distance;
                var invoiceQuantity = cars[i].Invoices.Select(i => i.Quantity).FirstOrDefault();
                var fuelAvailability = cars[i].InitialFuel + invoiceQuantity;
                var fuelConsumption = Math.Round(travelledDistance * (cars[i].AverageConsumption / 100.00), 2);

                var transportWorkTicket = new TransportWorkTicket
                {
                    Date = DateTime.UtcNow,
                    UserId = cars[i].CarUsers.Select(cu => cu.UserId).FirstOrDefault(),
                    CarId = cars[i].Id,
                    CreatedBy = cars[i].CarUsers.Select(cu => cu.ApplicationUser.FullName).FirstOrDefault(),
                    StartKilometers
                 = cars[i].StartKilometers,
                    TravelledDistance = travelledDistance,
                    EndKilometers = cars[i].StartKilometers + routes[i].Distance,
                    FuelAvailability = fuelAvailability,
                    FuelConsumption = fuelConsumption,
                    Residue = fuelAvailability - fuelConsumption,
                };

                await dbContext.TransportWorkTickets.AddAsync(transportWorkTicket);

                await dbContext.RouteTransportWorkTickets.AddAsync(new RouteTransportWorkTicket { RouteId = routes[i].Id, TransportWorkTicketId = transportWorkTicket.Id });
            }

            var companyIdTwo = dbContext.Companies
                .Where(c => c.Name == "ЕT Саламандър")
                .Select(c => c.Id)
                .FirstOrDefault();

            var carsTwo = dbContext.Cars
                .Where(c => c.CompanyId == companyIdTwo)
                .Include(c => c.Invoices)
                .Include(c => c.CarUsers)
                .ToList();

            for (int i = 0; i < carsTwo.Count; i++)
            {
                var travelledDistance = routes[i].Distance;
                var invoiceQuantity = carsTwo[i].Invoices.Select(i => i.Quantity).FirstOrDefault();
                var fuelAvailability = carsTwo[i].InitialFuel + invoiceQuantity;
                var fuelConsumption = Math.Round(travelledDistance * (carsTwo[i].AverageConsumption / 100.00), 2);

                var transportWorkTicket = new TransportWorkTicket
                {
                    Date = DateTime.UtcNow,
                    UserId = carsTwo[i].CarUsers.Select(cu => cu.UserId).FirstOrDefault(),
                    CarId = carsTwo[i].Id,
                    CreatedBy = carsTwo[i].CarUsers.Select(cu => cu.ApplicationUser.FullName).FirstOrDefault(),
                    StartKilometers
                 = carsTwo[i].StartKilometers,
                    TravelledDistance = travelledDistance,
                    EndKilometers = carsTwo[i].StartKilometers + routes[i].Distance,
                    FuelAvailability = fuelAvailability,
                    FuelConsumption = fuelConsumption,
                    Residue = fuelAvailability - fuelConsumption,
                };

                await dbContext.TransportWorkTickets.AddAsync(transportWorkTicket);

                await dbContext.RouteTransportWorkTickets.AddAsync(new RouteTransportWorkTicket { RouteId = routes[i].Id, TransportWorkTicketId = transportWorkTicket.Id });
            }
        }
    }
}
