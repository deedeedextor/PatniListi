namespace PatniListi.Services.Data.Tests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PatniListi.Data;
    using PatniListi.Data.Models;
    using PatniListi.Data.Repositories;
    using PatniListi.Services.Mapping;
    using PatniListi.Web.ViewModels.Administration.Cars;
    using PatniListi.Web.ViewModels.Administration.Users;
    using Xunit;

    public class CarsServiceTests
    {
        [Fact]
        public async Task CreateAsyncAddsCar()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Car>(new ApplicationDbContext(options.Options));
            var car = new Car { Model = "Форд Фиеста", LicensePlate = "CO1212KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 };

            var carsService = new CarsService(repository);
            await carsService.CreateAsync(car);

            var cars = repository.AllAsNoTracking().ToList().Count();

            Assert.Equal(1, cars);
        }

        [Fact]
        public async Task DeleteAsyncReturnsTrueIfCarExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Car>(new ApplicationDbContext(options.Options));

            var carOne = new Car { Model = "Форд Фиеста", LicensePlate = "CO1212KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 };
            var carTwo = new Car { Model = "Форд Фиеста", LicensePlate = "CO4312KA", CompanyId = "72804eud-3464-hfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Бензин, TankCapacity = 55, AverageConsumption = 6, InitialFuel = 10, StartKilometers = 230444 };

            await repository.AddAsync(carOne);
            await repository.AddAsync(carTwo);
            await repository.SaveChangesAsync();
            var carsService = new CarsService(repository);

            var isDeleted = await carsService.DeleteAsync(carOne.Id, "Петър Петров");
            var carsCount = repository.AllAsNoTracking().Count();

            Assert.Equal(1, carsCount);
            Assert.True(isDeleted);
        }

        [Fact]
        public async Task DeleteAsyncReturnsFalseIfCarDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Car>(new ApplicationDbContext(options.Options));

            var carOne = new Car { Model = "Форд Фиеста", LicensePlate = "CO1212KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 };
            var carTwo = new Car { Model = "Форд Фиеста", LicensePlate = "CO4312KA", CompanyId = "72804eud-3464-hfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Бензин, TankCapacity = 55, AverageConsumption = 6, InitialFuel = 10, StartKilometers = 230444 };

            await repository.AddAsync(carOne);
            await repository.AddAsync(carTwo);
            await repository.SaveChangesAsync();
            var carsService = new CarsService(repository);

            var isDeleted = await carsService.DeleteAsync("4738-djsk-r4-3456", "Петър Петров");
            var carsCount = repository.AllAsNoTracking().Count();

            Assert.Equal(2, carsCount);
            Assert.False(isDeleted);
        }

        [Fact]
        public async Task EditAsyncUpdatesCarInfo()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Car>(new ApplicationDbContext(options.Options));

            var carOne = new Car { Model = "Форд Фиеста", LicensePlate = "CO1212KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 };
            var carTwo = new Car { Model = "Форд Фиеста", LicensePlate = "CO4312KA", CompanyId = "72804eud-3464-hfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Бензин, TankCapacity = 55, AverageConsumption = 6, InitialFuel = 10, StartKilometers = 230444 };

            await repository.AddAsync(carOne);
            await repository.AddAsync(carTwo);
            await repository.SaveChangesAsync();
            var carsService = new CarsService(repository);

            carOne.Model = "Форд Фиеста 3";
            carOne.StartKilometers = 233444;

            await carsService.EditAsync(carOne, "Стоян Стоянов");
            var car = repository.AllAsNoTracking().FirstOrDefault(c => c.Id == carOne.Id);

            Assert.Equal(carOne.Id, car.Id);
            Assert.Equal(carOne.Model, car.Model);
            Assert.Equal(carOne.LicensePlate, car.LicensePlate);
            Assert.Equal(carOne.CompanyId, car.CompanyId);
            Assert.Equal(carOne.FuelType, car.FuelType);
            Assert.Equal(carOne.StartKilometers, car.StartKilometers);
        }

        [Fact]
        public void GetAllCarsAsQueryable()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Car>(new ApplicationDbContext(options.Options));

            var carOne = new Car { Model = "Форд Фиеста", LicensePlate = "CO1212KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 };
            var carTwo = new Car { Model = "Форд Фиеста", LicensePlate = "CO4312KA", CompanyId = "72804eud-3464-hfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Бензин, TankCapacity = 55, AverageConsumption = 6, InitialFuel = 10, StartKilometers = 230444 };
            var carThree = new Car { Model = "Форд Фиеста 8", LicensePlate = "CO9812KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 5, InitialFuel = 10, StartKilometers = 234957 };

            repository.AddAsync(carOne);
            repository.AddAsync(carTwo);
            repository.AddAsync(carThree);
            repository.SaveChangesAsync();
            var carsService = new CarsService(repository);

            AutoMapperConfig.RegisterMappings(typeof(CarViewModel).Assembly);
            var cars = carsService.GetAll<CarViewModel>(carThree.CompanyId).ToList();

            Assert.Equal(2, cars.Count());
        }

        [Fact]
        public void GetCarsByUser()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Car>(new ApplicationDbContext(options.Options));

            var carOne = new Car { Model = "Форд Фиеста", LicensePlate = "CO1212KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 };
            carOne.CarUsers.Add(new CarUser {CarId = carOne.Id, UserId = "242hds-78dsd-7823dsds" });
            var carTwo = new Car { Model = "Форд Фиеста", LicensePlate = "CO4312KA", CompanyId = "72804eud-3464-hfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Бензин, TankCapacity = 55, AverageConsumption = 6, InitialFuel = 10, StartKilometers = 230444 };
            carTwo.CarUsers.Add(new CarUser { CarId = carTwo.Id, UserId = "242hds-78dhgf-7823dsds" });
            carTwo.CarUsers.Add(new CarUser { CarId = carTwo.Id, UserId = "242tre-78dhgf-7823dsds" });
            var carThree = new Car { Model = "Форд Фиеста 8", LicensePlate = "CO9812KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 5, InitialFuel = 10, StartKilometers = 234957 };
            var carUser = new CarUser { CarId = carThree.Id, UserId = "242tre-78dhgf-7823dsds" };
            carThree.CarUsers.Add(carUser);

            repository.AddAsync(carOne);
            repository.AddAsync(carTwo);
            repository.AddAsync(carThree);
            repository.SaveChangesAsync();
            var carsService = new CarsService(repository);

            AutoMapperConfig.RegisterMappings(typeof(CarViewModel).Assembly);
            var cars = carsService.GetCarsByUser<CarViewModel>(carUser.UserId, carThree.CompanyId).ToList();

            Assert.Single(cars);
        }

        [Fact]
        public void GetAllCarsAsSelectListItem()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Car>(new ApplicationDbContext(options.Options));

            var carOne = new Car { Model = "Форд Фиеста", LicensePlate = "CO1212KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 };
            carOne.CarUsers.Add(new CarUser { CarId = carOne.Id, UserId = "242hds-78dsd-7823dsds" });
            var carTwo = new Car { Model = "Форд Фиеста", LicensePlate = "CO4312KA", CompanyId = "72804eud-3464-hfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Бензин, TankCapacity = 55, AverageConsumption = 6, InitialFuel = 10, StartKilometers = 230444 };
            carTwo.CarUsers.Add(new CarUser { CarId = carTwo.Id, UserId = "242hds-78dhgf-7823dsds" });
            carTwo.CarUsers.Add(new CarUser { CarId = carTwo.Id, UserId = "242tre-78dhgf-7823dsds" });
            var carThree = new Car { Model = "Форд Фиеста 8", LicensePlate = "CO9812KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 5, InitialFuel = 10, StartKilometers = 234957 };
            carThree.CarUsers.Add(new CarUser { CarId = carThree.Id, UserId = "242tre-78dhgf-7823dsds" });

            repository.AddAsync(carOne);
            repository.AddAsync(carTwo);
            repository.AddAsync(carThree);
            repository.SaveChangesAsync();
            var carsService = new CarsService(repository);

            var cars = carsService.GetAll(carOne.CompanyId);

            Assert.Equal(2, cars.Count());
        }

        [Fact]
        public void GetAllCarsByUserIdAsSelectListitem()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Car>(new ApplicationDbContext(options.Options));

            var carOne = new Car { Model = "Форд Фиеста", LicensePlate = "CO1212KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 };
            carOne.CarUsers.Add(new CarUser { CarId = carOne.Id, UserId = "242hds-78dsd-7823dsds" });
            var carTwo = new Car { Model = "Форд Фиеста", LicensePlate = "CO4312KA", CompanyId = "72804eud-3464-hfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Бензин, TankCapacity = 55, AverageConsumption = 6, InitialFuel = 10, StartKilometers = 230444 };
            carTwo.CarUsers.Add(new CarUser { CarId = carTwo.Id, UserId = "242hds-78dhgf-7823dsds" });
            carTwo.CarUsers.Add(new CarUser { CarId = carTwo.Id, UserId = "242tre-78dhgf-7823dsds" });
            var carThree = new Car { Model = "Форд Фиеста 8", LicensePlate = "CO9812KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 5, InitialFuel = 10, StartKilometers = 234957 };
            carThree.CarUsers.Add(new CarUser { CarId = carThree.Id, UserId = "242tre-78dhgf-7823dsds" });

            repository.AddAsync(carOne);
            repository.AddAsync(carTwo);
            repository.AddAsync(carThree);
            repository.SaveChangesAsync();
            var carsService = new CarsService(repository);

            var cars = carsService.GetAllCarsByUserId("242tre-78dhgf-7823dsds", carOne.CompanyId);

            Assert.Single(cars);
        }

        [Fact]
        public void GetCurrentLitresByCarId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Car>(new ApplicationDbContext(options.Options));

            var carOne = new Car { Model = "Форд Фиеста", LicensePlate = "CO1212KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 };
            carOne.Invoices.Add(new Invoice { Number = "43254325235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 10, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", Location = "София", Price = 2.09m, Quantity = 25.21, TotalPrice = 2.09m * 25.21m });
            carOne.Invoices.Add(new Invoice { Number = "11254325235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 25, UserId = "331414153", CarId = "72804eudajhkhfvs-dasfa", Location = "София", Price = 2.07m, Quantity = 45.21, TotalPrice = 2.09m * 25.21m });
            var carTwo = new Car { Model = "Форд Фиеста", LicensePlate = "CO4312KA", CompanyId = "72804eud-3464-hfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Бензин, TankCapacity = 55, AverageConsumption = 6, InitialFuel = 10, StartKilometers = 230444 };
            carTwo.Invoices.Add(new Invoice { Number = "43258825235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 20, UserId = "221414153", CarId = "72804eud-3464-hfvs-dasfa", Location = "Варна", Price = 2.06m, Quantity = 22.21, TotalPrice = 2.06m * 22.21m });

            repository.AddAsync(carOne);
            repository.AddAsync(carTwo);
            repository.SaveChangesAsync();
            var carsService = new CarsService(repository);

            var liters = carsService.GetCurrentLitresByCarId(carOne.Id);

            Assert.Equal(70.42, liters);
        }

        [Fact]
        public void GetCurrentTravelledDistanceByCarId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Car>(new ApplicationDbContext(options.Options));

            var carOne = new Car { Model = "Форд Фиеста", LicensePlate = "CO1212KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 };
            carOne.TransportWorkTickets.Add(new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200100, TravelledDistance = 441, EndKilometers = 200541, FuelAvailability = 55, FuelConsumption = 441 * (5 / 100), Residue = 55 - (441 * (5 / 100)) });
            carOne.TransportWorkTickets.Add(new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200541, TravelledDistance = 100, EndKilometers = 200641, FuelAvailability = 55, FuelConsumption = 100 * (5 / 100), Residue = 55 - (100 * (5 / 100)) });
            var carTwo = new Car { Model = "Форд Фиеста", LicensePlate = "CO4312KA", CompanyId = "72804eud-3464-hfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Бензин, TankCapacity = 55, AverageConsumption = 6, InitialFuel = 10, StartKilometers = 230444 };
            carTwo.TransportWorkTickets.Add(new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414188", CarId = "72804eud-3464-hfvs-dasfa", StartKilometers = 200541, TravelledDistance = 200, EndKilometers = 200741, FuelAvailability = 55, FuelConsumption = 200 * (5 / 100), Residue = 55 - (200 * (5 / 100)) });

            repository.AddAsync(carOne);
            repository.AddAsync(carTwo);
            repository.SaveChangesAsync();
            var carsService = new CarsService(repository);

            var travelledDistanceDb = repository
                            .AllAsNoTracking()
                            .Where(c => c.Id == carOne.Id)
                            .Select(i => i.TransportWorkTickets.Sum(i => i.TravelledDistance))
                            .SingleOrDefault();
            var travelledDistance = carsService.GetCurrentTravelledDistanceByCarId(carOne.Id);

            Assert.Equal(travelledDistanceDb, travelledDistance);
        }

        [Fact]
        public void GetCurrentTravelledDistanceByCarIdExceptForTheCurrentTransportWorkTicketId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Car>(new ApplicationDbContext(options.Options));

            var carOne = new Car { Model = "Форд Фиеста", LicensePlate = "CO1212KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 };
            var workTicket = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341455153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200100, TravelledDistance = 441, EndKilometers = 200541, FuelAvailability = 55, FuelConsumption = 441 * (5 / 100), Residue = 55 - (441 * (5 / 100)) };
            carOne.TransportWorkTickets.Add(workTicket);
            var workTicketOne = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200541, TravelledDistance = 100, EndKilometers = 200641, FuelAvailability = 55, FuelConsumption = 100 * (5 / 100), Residue = 55 - (100 * (5 / 100)) };
            carOne.TransportWorkTickets.Add(workTicketOne);
            var carTwo = new Car { Model = "Форд Фиеста", LicensePlate = "CO4312KA", CompanyId = "72804eud-3464-hfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Бензин, TankCapacity = 55, AverageConsumption = 6, InitialFuel = 10, StartKilometers = 230444 };
            var workTicketTwo = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414188", CarId = "72804eud-3464-hfvs-dasfa", StartKilometers = 200541, TravelledDistance = 200, EndKilometers = 200741, FuelAvailability = 55, FuelConsumption = 200 * (5 / 100), Residue = 55 - (200 * (5 / 100)) };
            carTwo.TransportWorkTickets.Add(workTicketTwo);

            repository.AddAsync(carOne);
            repository.AddAsync(carTwo);
            repository.SaveChangesAsync();
            var carsService = new CarsService(repository);

            var travelledDistance = carsService.GetCurrentTravelledDistanceByCarId(carOne.Id, workTicket.Id);

            Assert.Equal(100, travelledDistance);
        }

        [Fact]
        public void GetCurrentFuelConsumptionByCarId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Car>(new ApplicationDbContext(options.Options));

            var carOne = new Car { Model = "Форд Фиеста", LicensePlate = "CO1212KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 };
            carOne.TransportWorkTickets.Add(new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200100, TravelledDistance = 441, EndKilometers = 200541, FuelAvailability = 55, FuelConsumption = 441 * (5 / 100), Residue = 55 - (441 * (5 / 100)) });
            carOne.TransportWorkTickets.Add(new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200541, TravelledDistance = 100, EndKilometers = 200641, FuelAvailability = 55, FuelConsumption = 100 * (5 / 100), Residue = 55 - (100 * (5 / 100)) });
            var carTwo = new Car { Model = "Форд Фиеста", LicensePlate = "CO4312KA", CompanyId = "72804eud-3464-hfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Бензин, TankCapacity = 55, AverageConsumption = 6, InitialFuel = 10, StartKilometers = 230444 };
            carTwo.TransportWorkTickets.Add(new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414188", CarId = "72804eud-3464-hfvs-dasfa", StartKilometers = 200541, TravelledDistance = 200, EndKilometers = 200741, FuelAvailability = 55, FuelConsumption = 200 * (5 / 100), Residue = 55 - (200 * (5 / 100)) });

            repository.AddAsync(carOne);
            repository.AddAsync(carTwo);
            repository.SaveChangesAsync();
            var carsService = new CarsService(repository);

            var fuelConsumptionDb = repository
                .AllAsNoTracking()
                .Where(c => c.Id == carOne.Id)
                .Select(tr => tr.TransportWorkTickets
                .Sum(tr => tr.FuelConsumption))
                .SingleOrDefault();

            var fuelConsumption = carsService.GetCurrentFuelConsumptionByCarId(carOne.Id);

            Assert.Equal(fuelConsumptionDb, fuelConsumption);
        }

        [Fact]
        public void GetCurrentFuelConsumptionByCarIdExceptForTheCurrentTransportWorkTicketId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Car>(new ApplicationDbContext(options.Options));

            var carOne = new Car { Model = "Форд Фиеста", LicensePlate = "CO1212KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 };
            var workTicket = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341455153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200100, TravelledDistance = 441, EndKilometers = 200541, FuelAvailability = 55, FuelConsumption = 441 * (5 / 100), Residue = 55 - (441 * (5 / 100)) };
            carOne.TransportWorkTickets.Add(workTicket);
            var workTicketOne = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", StartKilometers = 200541, TravelledDistance = 100, EndKilometers = 200641, FuelAvailability = 55, FuelConsumption = 100 * (5 / 100), Residue = 55 - (100 * (5 / 100)) };
            carOne.TransportWorkTickets.Add(workTicketOne);
            var carTwo = new Car { Model = "Форд Фиеста", LicensePlate = "CO4312KA", CompanyId = "72804eud-3464-hfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Бензин, TankCapacity = 55, AverageConsumption = 6, InitialFuel = 10, StartKilometers = 230444 };
            var workTicketTwo = new TransportWorkTicket { Date = DateTime.UtcNow, UserId = "341414188", CarId = "72804eud-3464-hfvs-dasfa", StartKilometers = 200541, TravelledDistance = 200, EndKilometers = 200741, FuelAvailability = 55, FuelConsumption = 200 * (5 / 100), Residue = 55 - (200 * (5 / 100)) };
            carTwo.TransportWorkTickets.Add(workTicketTwo);

            repository.AddAsync(carOne);
            repository.AddAsync(carTwo);
            repository.SaveChangesAsync();
            var carsService = new CarsService(repository);

            var fuelConsumptionFromDb = repository
                          .AllAsNoTracking()
                          .Where(c => c.Id == carOne.Id)
                          .Select(i => i.TransportWorkTickets.Where(tr => tr.Id != workTicket.Id).Sum(i => i.FuelConsumption))
                          .SingleOrDefault();

            var travelledDistance = carsService.GetCurrentFuelConsumptionByCarId(carOne.Id, workTicket.Id);

            Assert.Equal(fuelConsumptionFromDb, travelledDistance);
        }

        [Fact]
        public async Task GetDetailsAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Car>(new ApplicationDbContext(options.Options));

            var carOne = new Car { Model = "Форд Фиеста", LicensePlate = "CO1212KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 };
            var carUserOne = new CarUser { CarId = carOne.Id, UserId = "242hds-78dsd-7823dsds" };
            carOne.CarUsers.Add(carUserOne);
            var carTwo = new Car { Model = "Форд Фиеста", LicensePlate = "CO4312KA", CompanyId = "72804eud-3464-hfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Бензин, TankCapacity = 55, AverageConsumption = 6, InitialFuel = 10, StartKilometers = 230444 };
            var carUserTwo = new CarUser { CarId = carTwo.Id, UserId = "242hds-78dhgf-7823dsds" };
            carTwo.CarUsers.Add(carUserTwo);
            var carUserTwoOne = new CarUser { CarId = carTwo.Id, UserId = "242tre-78dhgf-7823dsds" };
            carTwo.CarUsers.Add(carUserTwoOne);
            var carThree = new Car { Model = "Форд Фиеста 8", LicensePlate = "CO9812KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 5, InitialFuel = 10, StartKilometers = 234957 };
            var carUserThree = new CarUser { CarId = carThree.Id, UserId = "242tre-78dhgf-7823dsds" };
            carThree.CarUsers.Add(carUserThree);

            await repository.AddAsync(carOne);
            await repository.AddAsync(carTwo);
            await repository.AddAsync(carThree);
            await repository.SaveChangesAsync();
            var carsService = new CarsService(repository);

            AutoMapperConfig.RegisterMappings(typeof(CarDetailsViewModel).Assembly);
            var car = await carsService.GetDetailsAsync<CarDetailsViewModel>(carThree.Id);

            Assert.Equal(carThree.Model, car.Model);
            Assert.Equal(carThree.LicensePlate, car.LicensePlate);
            Assert.Equal(carThree.CarUsers.Count(), car.AllDrivers.Count());
        }

        [Fact]
        public void IsLicensePlateExistReturnsTrue()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Car>(new ApplicationDbContext(options.Options));

            var carOne = new Car { Model = "Форд Фиеста", LicensePlate = "CO1212KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 };
            var carTwo = new Car { Model = "Форд Фиеста", LicensePlate = "CO4312KA", CompanyId = "72804eud-3464-hfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Бензин, TankCapacity = 55, AverageConsumption = 6, InitialFuel = 10, StartKilometers = 230444 };
            var carThree = new Car { Model = "Форд Фиеста 8", LicensePlate = "CO9812KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 5, InitialFuel = 10, StartKilometers = 234957 };

            repository.AddAsync(carOne);
            repository.AddAsync(carTwo);
            repository.AddAsync(carThree);
            repository.SaveChangesAsync();
            var carsService = new CarsService(repository);

            var exists = carsService.IsLicensePlateExist(carTwo.LicensePlate);

            Assert.True(exists);
        }

        [Fact]
        public void IsLicensePlateExistReturnsFalse()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Car>(new ApplicationDbContext(options.Options));

            var carOne = new Car { Model = "Форд Фиеста", LicensePlate = "CO1212KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 };
            var carTwo = new Car { Model = "Форд Фиеста", LicensePlate = "CO4312KA", CompanyId = "72804eud-3464-hfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Бензин, TankCapacity = 55, AverageConsumption = 6, InitialFuel = 10, StartKilometers = 230444 };
            var carThree = new Car { Model = "Форд Фиеста 8", LicensePlate = "CO9812KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 5, InitialFuel = 10, StartKilometers = 234957 };

            repository.AddAsync(carOne);
            repository.AddAsync(carTwo);
            repository.AddAsync(carThree);
            repository.SaveChangesAsync();
            var carsService = new CarsService(repository);

            var exists = carsService.IsLicensePlateExist("CO9862KA");

            Assert.False(exists);
        }

        [Fact]
        public void GetLicensePlateById()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Car>(new ApplicationDbContext(options.Options));

            var carOne = new Car { Model = "Форд Фиеста", LicensePlate = "CO1212KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 };
            var carTwo = new Car { Model = "Форд Фиеста", LicensePlate = "CO4312KA", CompanyId = "72804eud-3464-hfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Бензин, TankCapacity = 55, AverageConsumption = 6, InitialFuel = 10, StartKilometers = 230444 };
            var carThree = new Car { Model = "Форд Фиеста 8", LicensePlate = "CO9812KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 5, InitialFuel = 10, StartKilometers = 234957 };

            repository.AddAsync(carOne);
            repository.AddAsync(carTwo);
            repository.AddAsync(carThree);
            repository.SaveChangesAsync();
            var carsService = new CarsService(repository);

            var licensePlateFromDb = repository.AllAsNoTracking().Where(c => c.LicensePlate == carOne.LicensePlate).Select(c => c.LicensePlate).FirstOrDefault();

            var licensePlate = carsService.GetLicensePlateById(carOne.Id);

            Assert.Equal(licensePlateFromDb, licensePlate);
        }

        [Fact]
        public void GetByIdReturnsCar()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Car>(new ApplicationDbContext(options.Options));

            var carOne = new Car { Model = "Форд Фиеста", LicensePlate = "CO1212KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 };
            var carTwo = new Car { Model = "Форд Фиеста", LicensePlate = "CO4312KA", CompanyId = "72804eud-3464-hfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Бензин, TankCapacity = 55, AverageConsumption = 6, InitialFuel = 10, StartKilometers = 230444 };
            var carThree = new Car { Model = "Форд Фиеста 8", LicensePlate = "CO9812KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 5, InitialFuel = 10, StartKilometers = 234957 };

            repository.AddAsync(carOne);
            repository.AddAsync(carTwo);
            repository.AddAsync(carThree);
            repository.SaveChangesAsync();
            var carsService = new CarsService(repository);

            var carFromDb = repository.AllAsNoTracking().FirstOrDefault(c => c.Id == carThree.Id);

            var car = carsService.GetById(carThree.Id);

            Assert.Equal(carFromDb.Model, car.Model);
            Assert.Equal(carFromDb.LicensePlate, car.LicensePlate);
            Assert.Equal(carFromDb.AverageConsumption, car.AverageConsumption);
        }
    }
}
