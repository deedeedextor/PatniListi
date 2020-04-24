using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PatniListi.Data;
using PatniListi.Data.Models;
using PatniListi.Data.Repositories;
using PatniListi.Services.Mapping;
using PatniListi.Web.ViewModels.Models.TransportWorkTickets;
using Xunit;

namespace PatniListi.Services.Data.Tests
{
    public class TransportWorkTicketsTests
    {

        [Fact]
        public async Task CreateAsyncAddsCar()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<TransportWorkTicket>(new ApplicationDbContext(options.Options));

            var transportWorkTicket = new TransportWorkTicket
            {
                Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa",
                StartKilometers = 200100, TravelledDistance = 441, EndKilometers = 200541, FuelAvailability = 55,
                FuelConsumption = 441 * (5 / 100), Residue = 55 - (441 * (5 / 100)),
            };

            var transportWorkTicketsService = new TransportWorkTicketsService(repository);

            await transportWorkTicketsService.CreateAsync(transportWorkTicket);

            var workTickets = repository.AllAsNoTracking().ToList();

            Assert.Single(workTickets);
        }

        [Fact]
        public async Task DeleteAsyncReturnsTrueIfTransportWorkTicketAlreadyExist()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<TransportWorkTicket>(new ApplicationDbContext(options.Options));

            var workTicketOne = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200100, TravelledDistance = 441, EndKilometers = 200541, FuelAvailability = 55, FuelConsumption = 441 * (5 / 100), Residue = 55 - (441 * (5 / 100)) };
            var workTicketTwo = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200541, TravelledDistance = 100, EndKilometers = 200641, FuelAvailability = 55, FuelConsumption = 100 * (5 / 100), Residue = 55 - (100 * (5 / 100)) };

            await repository.AddAsync(workTicketOne);
            await repository.AddAsync(workTicketTwo);
            await repository.SaveChangesAsync();
            var transportWorkTicketsService = new TransportWorkTicketsService(repository);

            var isDeleted = await transportWorkTicketsService.DeleteAsync(workTicketOne.Id, "Силвия Петрова");
            var cars = repository.AllAsNoTracking();

            Assert.Single(cars);
            Assert.True(isDeleted);
        }

        [Fact]
        public async Task DeleteAsyncReturnsFalseIfTransportWorkTicketDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<TransportWorkTicket>(new ApplicationDbContext(options.Options));

            var workTicketOne = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200100, TravelledDistance = 441, EndKilometers = 200541, FuelAvailability = 55, FuelConsumption = 441 * (5 / 100), Residue = 55 - (441 * (5 / 100)) };
            var workTicketTwo = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200541, TravelledDistance = 100, EndKilometers = 200641, FuelAvailability = 55, FuelConsumption = 100 * (5 / 100), Residue = 55 - (100 * (5 / 100)) };

            await repository.AddAsync(workTicketOne);
            await repository.AddAsync(workTicketTwo);
            await repository.SaveChangesAsync();
            var transportWorkTicketsService = new TransportWorkTicketsService(repository);

            var isDeleted = await transportWorkTicketsService.DeleteAsync("4738-djsk-r4-3456", "Силвия Петрова");
            var carsCount = repository.AllAsNoTracking().Count();

            Assert.Equal(2, carsCount);
            Assert.False(isDeleted);
        }

        [Fact]
        public async Task EditAsyncUpdatesTransportWorkTicketInfo()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<TransportWorkTicket>(new ApplicationDbContext(options.Options));

            var workTicketOne = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200100, TravelledDistance = 441, EndKilometers = 200541, FuelAvailability = 55, FuelConsumption = 441 * (5 / 100), Residue = 55 - (441 * (5 / 100)) };
            var workTicketTwo = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200541, TravelledDistance = 100, EndKilometers = 200641, FuelAvailability = 55, FuelConsumption = 100 * (5 / 100), Residue = 55 - (100 * (5 / 100)) };

            await repository.AddAsync(workTicketOne);
            await repository.AddAsync(workTicketTwo);
            await repository.SaveChangesAsync();
            var transportWorkTicketsService = new TransportWorkTicketsService(repository);

            workTicketTwo.StartKilometers = 200700;
            workTicketTwo.TravelledDistance = 200;

            await transportWorkTicketsService.EditAsync(workTicketTwo);
            var workTicket = repository.AllAsNoTracking().FirstOrDefault(tr => tr.Id == workTicketTwo.Id);

            Assert.Equal(workTicketTwo.Date, workTicket.Date);
            Assert.Equal(workTicketTwo.StartKilometers, workTicket.StartKilometers);
            Assert.Equal(workTicketTwo.TravelledDistance, workTicket.TravelledDistance);
        }

        [Fact]
        public void GetAllTransportWorkTicketsForPeriodAsQueryable()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<TransportWorkTicket>(new ApplicationDbContext(options.Options));

            var workTicketOne = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200100, TravelledDistance = 441, EndKilometers = 200541, FuelAvailability = 55, FuelConsumption = 441 * (5 / 100), Residue = 55 - (441 * (5 / 100)) };
            var workTicketTwo = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200541, TravelledDistance = 100, EndKilometers = 200641, FuelAvailability = 55, FuelConsumption = 100 * (5 / 100), Residue = 55 - (100 * (5 / 100)) };

            repository.AddAsync(workTicketOne);
            repository.AddAsync(workTicketTwo);
            repository.SaveChangesAsync();

            var transportWorkTicketsService = new TransportWorkTicketsService(repository);

            AutoMapperConfig.RegisterMappings(typeof(TransportWorkTicketViewModel).Assembly);
            var workTickets = transportWorkTicketsService.GetAllTransportWorkTicketsForPeriod<TransportWorkTicketViewModel>(workTicketTwo.CarId, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow);

            Assert.Equal(2, workTickets.Count());
        }

        [Fact]
        public async Task GetDetailsAsyncReturnsTransportWorkTicketInfo()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<TransportWorkTicket>(new ApplicationDbContext(options.Options));

            var workTicketOne = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200100, TravelledDistance = 441, EndKilometers = 200541, FuelAvailability = 55, FuelConsumption = 441 * (5 / 100), Residue = 55 - (441 * (5 / 100)) };
            var workTicketTwo = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200541, TravelledDistance = 100, EndKilometers = 200641, FuelAvailability = 55, FuelConsumption = 100 * (5 / 100), Residue = 55 - (100 * (5 / 100)) };

            await repository.AddAsync(workTicketOne);
            await repository.AddAsync(workTicketTwo);
            await repository.SaveChangesAsync();

            var transportWorkTicketsService = new TransportWorkTicketsService(repository);

            AutoMapperConfig.RegisterMappings(typeof(TransportWorkTicketViewModel).Assembly);
            var workTicket = await
                transportWorkTicketsService.GetDetailsAsync<TransportWorkTicketDetailsViewModel>(workTicketOne.Id);

            Assert.Equal(workTicketOne.StartKilometers, workTicket.StartKilometers);
            Assert.Equal(workTicketOne.EndKilometers, workTicket.EndKilometers);
            Assert.Equal(workTicketOne.TravelledDistance, workTicket.TravelledDistance);
            Assert.Equal(workTicketOne.FuelAvailability, workTicket.FuelAvailability);
        }

        [Fact]
        public async Task GetByIdReturnsTransportWorkTicket()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<TransportWorkTicket>(new ApplicationDbContext(options.Options));

            var workTicketOne = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200100, TravelledDistance = 441, EndKilometers = 200541, FuelAvailability = 55, FuelConsumption = 441 * (5 / 100), Residue = 55 - (441 * (5 / 100)) };
            var workTicketTwo = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200541, TravelledDistance = 100, EndKilometers = 200641, FuelAvailability = 55, FuelConsumption = 100 * (5 / 100), Residue = 55 - (100 * (5 / 100)) };
            var workTicketThree = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "3414141890", CarId = "72804eu-jhkhfvs-dasfa", StartKilometers = 200800, TravelledDistance = 100, EndKilometers = 200900, FuelAvailability = 55, FuelConsumption = 100 * (5 / 100), Residue = 55 - (100 * (5 / 100)) };

            await repository.AddAsync(workTicketOne);
            await repository.AddAsync(workTicketTwo);
            await repository.SaveChangesAsync();

            var transportWorkTicketsService = new TransportWorkTicketsService(repository);

            AutoMapperConfig.RegisterMappings(typeof(TransportWorkTicketViewModel).Assembly);
            var workTicket =
                transportWorkTicketsService.GetById(workTicketOne.Id);

            Assert.Equal(workTicketOne.StartKilometers, workTicket.StartKilometers);
            Assert.Equal(workTicketOne.EndKilometers, workTicket.EndKilometers);
            Assert.Equal(workTicketOne.TravelledDistance, workTicket.TravelledDistance);
            Assert.Equal(workTicketOne.Date, workTicket.Date);
            Assert.Equal(workTicketOne.FuelAvailability, workTicket.FuelAvailability);
            Assert.Equal(workTicketOne.FuelConsumption, workTicket.FuelConsumption);
        }
    }
}
