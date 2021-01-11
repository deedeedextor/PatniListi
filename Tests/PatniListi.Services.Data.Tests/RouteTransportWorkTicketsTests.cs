namespace PatniListi.Services.Data.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Moq;
    using PatniListi.Data;
    using PatniListi.Data.Models;
    using PatniListi.Data.Repositories;
    using PatniListi.Services.Mapping;
    using PatniListi.Web.ViewModels.Models.Routes;
    using Xunit;

    public class RouteTransportWorkTicketsTests
    {
        [Fact]
        public async Task SetIsDeletedAsyncChangesUsersForCars()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<RouteTransportWorkTicket>(new ApplicationDbContext(options.Options));

            var workTicketOne = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200100, TravelledDistance = 441, EndKilometers = 200541, FuelAvailability = 55, FuelConsumption = 441 * (5 / 100), Residue = 55 - (441 * (5 / 100)) };
            workTicketOne.RouteTransportWorkTickets.Add(new RouteTransportWorkTicket { TransportWorkTicketId = workTicketOne.Id, RouteId = "242hds-78dsd-7823dsds", IsDeleted = false });

            var workTicketTwo = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200541, TravelledDistance = 100, EndKilometers = 200641, FuelAvailability = 55, FuelConsumption = 100 * (5 / 100), Residue = 55 - (100 * (5 / 100)) };
            workTicketTwo.RouteTransportWorkTickets.Add(new RouteTransportWorkTicket { TransportWorkTicketId = workTicketTwo.Id, RouteId = "242hds-78dhgf-7823dsds", IsDeleted = false });
            workTicketTwo.RouteTransportWorkTickets.Add(new RouteTransportWorkTicket { TransportWorkTicketId = workTicketTwo.Id, RouteId = "242tre-78dh00-7823dsds", IsDeleted = false });

            var workTicketThree = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "3414141890", CarId = "72804eu-jhkhfvs-dasfa", StartKilometers = 200800, TravelledDistance = 100, EndKilometers = 200900, FuelAvailability = 55, FuelConsumption = 100 * (5 / 100), Residue = 55 - (100 * (5 / 100)) };
            workTicketThree.RouteTransportWorkTickets.Add(new RouteTransportWorkTicket { TransportWorkTicketId = workTicketThree.Id, RouteId = "242tre-78dhgf-7823dsds", IsDeleted = false });

            var fullName = "Мая Малинова";

            var routesService = new Mock<IRoutesService>();
            var routeTransportWorkTicketsService = new RouteTransportWorkTisketsService(repository, routesService.Object);

            AutoMapperConfig.RegisterMappings(typeof(RouteTransportViewModel).Assembly);
            await routeTransportWorkTicketsService.SetIsDeletedAsync(workTicketThree.Id, fullName);

            var workTickets = repository.AllAsNoTracking().FirstOrDefault(tr => tr.Id == workTicketThree.Id);

            Assert.Null(workTickets);
        }

        /*
        [Fact]
        public async Task UpdateAsyncAddNewUsersToCar()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<RouteTransportWorkTicket>(new ApplicationDbContext(options.Options));
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var workTicketOne = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200100, TravelledDistance = 441, EndKilometers = 200541, FuelAvailability = 55, FuelConsumption = 441 * (5 / 100), Residue = 55 - (441 * (5 / 100)) };
            workTicketOne.RouteTransportWorkTickets.Add(new RouteTransportWorkTicket { TransportWorkTicketId = workTicketOne.Id, RouteId = "242hds-78dsd-7823dsds", IsDeleted = false });

            var workTicketTwo = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200541, TravelledDistance = 100, EndKilometers = 200641, FuelAvailability = 55, FuelConsumption = 100 * (5 / 100), Residue = 55 - (100 * (5 / 100)) };
            workTicketTwo.RouteTransportWorkTickets.Add(new RouteTransportWorkTicket { TransportWorkTicketId = workTicketTwo.Id, RouteId = "242hds-78dhgf-7823dsds", IsDeleted = false });
            workTicketTwo.RouteTransportWorkTickets.Add(new RouteTransportWorkTicket { TransportWorkTicketId = workTicketTwo.Id, RouteId = "242tre-78dh00-7823dsds", IsDeleted = false });

            var workTicketThree = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "3414141890", CarId = "72804eu-jhkhfvs-dasfa", StartKilometers = 200800, TravelledDistance = 100, EndKilometers = 200900, FuelAvailability = 55, FuelConsumption = 100 * (5 / 100), Residue = 55 - (100 * (5 / 100)) };
            workTicketThree.RouteTransportWorkTickets.Add(new RouteTransportWorkTicket { TransportWorkTicketId = workTicketThree.Id, RouteId = "242tre-78dhgf-7823dsds", IsDeleted = false });

            var user = new ApplicationUser { UserName = "rima32", Email = "petrov.12@gmail.com", CompanyId = "7480-32141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };

            await usersRepository.AddAsync(user);
            await usersRepository.SaveChangesAsync();

            IEnumerable<string> drivers = new List<string>()
            {
                "Петър Петров"
            };

            var routesService = new Mock<IRoutesService>();
            var routeTransportWorkTicketsService = new RouteTransportWorkTisketsService(repository, routesService.Object);

            AutoMapperConfig.RegisterMappings(typeof(RouteViewModel).Assembly);
            AutoMapperConfig.RegisterMappings(typeof(RouteTransportViewModel).Assembly);
            await routeTransportWorkTicketsService.UpdateAsync(workTicketTwo.Id, workTicketTwo.Car.CompanyId, drivers);

            Assert.Single(workTicketTwo.RouteTransportWorkTickets);
        }

        [Fact]
        public async Task GetAllAsCollectionByCar()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<RouteTransportWorkTicket>(new ApplicationDbContext(options.Options));

            var workTicketOne = new RouteTransportWorkTicket { TransportWorkTicketId = "889q7423695", RouteId = "242hds-78dhgf-7823dsds", IsDeleted = false };
            var workTicketTwo = new RouteTransportWorkTicket { TransportWorkTicketId = "2147875cdnb", RouteId = "242tre-78dh00-7823dsds", IsDeleted = false };

            await repository.AddAsync(workTicketOne);
            await repository.AddAsync(workTicketTwo);
            await repository.SaveChangesAsync();

            var routesService = new Mock<IRoutesService>();
            var routeTransportWorkTicketsService = new RouteTransportWorkTisketsService(repository, routesService.Object);

            AutoMapperConfig.RegisterMappings(typeof(RouteTransportViewModel).Assembly);
            var routeWorkTickets = await routeTransportWorkTicketsService.GetAllAsync<RouteTransportViewModel>(workTicketOne.Id);

            Assert.Single(routeWorkTickets);
        }
        */
    }
}
