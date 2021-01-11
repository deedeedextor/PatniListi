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
    using PatniListi.Web.ViewModels.Administration.Cars;
    using PatniListi.Web.ViewModels.Administration.Users;
    using Xunit;

    public class CarUsersServiceTests
    {
        [Fact]
        public async Task SetIsDeletedAsyncChangesUsersForCars()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<CarUser>(new ApplicationDbContext(options.Options));

            var carOne = new Car { Model = "Форд Фиеста", LicensePlate = "CO1212KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 };
            carOne.CarUsers.Add(new CarUser { CarId = carOne.Id, UserId = "242hds-78dsd-7823dsds", IsDeleted = false });
            var carTwo = new Car { Model = "Форд Фиеста", LicensePlate = "CO4312KA", CompanyId = "72804eud-3464-hfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Бензин, TankCapacity = 55, AverageConsumption = 6, InitialFuel = 10, StartKilometers = 230444 };
            carTwo.CarUsers.Add(new CarUser { CarId = carTwo.Id, UserId = "242hds-78dhgf-7823dsds", IsDeleted = false });
            carTwo.CarUsers.Add(new CarUser { CarId = carTwo.Id, UserId = "242tre-78dhgf-7823dsds", IsDeleted = false });
            var carThree = new Car { Model = "Форд Фиеста 8", LicensePlate = "CO9812KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 5, InitialFuel = 10, StartKilometers = 234957 };
            carThree.CarUsers.Add(new CarUser { CarId = carThree.Id, UserId = "242tre-78dhgf-7823dsds", IsDeleted = false });

            var fullName = "Мая Маринова";

            var usersService = new Mock<IUsersService>();
            var carUsersService = new CarUsersService(repository, usersService.Object);

            await carUsersService.SetIsDeletedAsync(carTwo.Id, fullName);

            var carsFromDb = repository.AllAsNoTracking().FirstOrDefault(c => c.Car.CompanyId == carTwo.CompanyId);

            Assert.Null(carsFromDb);
        }

        [Fact]
        public async Task UpdateAsyncAddNewUsersToCar()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<CarUser>(new ApplicationDbContext(options.Options));
            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var carsRepository = new EfDeletableEntityRepository<Car>(new ApplicationDbContext(options.Options));

            var carTwo = new Car { Model = "Форд Фиеста", LicensePlate = "CO4312KA", CompanyId = "72804eud-3464-hfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Бензин, TankCapacity = 55, AverageConsumption = 6, InitialFuel = 10, StartKilometers = 230444 };
            var user = new ApplicationUser { UserName = "rima32", Email = "petrov.12@gmail.com", CompanyId = "7480-32141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };
            var userOne = new ApplicationUser { UserName = "rima11", Email = "p.12@gmail.com", CompanyId = "7sd0-32141-3274983", FullName = "Мая Маринова", LastLoggingDate = DateTime.UtcNow };
            await usersRepository.AddAsync(user);
            await usersRepository.AddAsync(userOne);
            await usersRepository.SaveChangesAsync();

            var carUserTwo = new CarUser { CarId = carTwo.Id, UserId = user.Id };
            carTwo.CarUsers.Add(carUserTwo);

            await carsRepository.AddAsync(carTwo);
            await carsRepository.SaveChangesAsync();

            IEnumerable<string> drivers = new List<string>()
            {
                "Мая Маринова",
                "Петър Петров",
            };

            var usersService = new Mock<IUsersService>();
            var carUsersService = new CarUsersService(repository, usersService.Object);

            AutoMapperConfig.RegisterMappings(typeof(UserCarViewModel).Assembly);
            await carUsersService.UpdateAsync(carTwo.Id, carTwo.CompanyId, drivers);

            Assert.Equal(2, carTwo.CarUsers.Count);
        }

        [Fact]
        public async Task GetAllAsCollectionByCar()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<CarUser>(new ApplicationDbContext(options.Options));

            var carUserOne = new CarUser { CarId = "123-098-hjk", UserId = "242hds-78dhgf-7823dsds" };
            var carUserTwo = new CarUser { CarId = "asd-123-rfv", UserId = "242tre-78dhgf-7823dsds" };

            await repository.AddAsync(carUserOne);
            await repository.AddAsync(carUserTwo);
            await repository.SaveChangesAsync();

            var usersService = new Mock<IUsersService>();
            var carUsersService = new CarUsersService(repository, usersService.Object);

            AutoMapperConfig.RegisterMappings(typeof(CarUserViewModel).Assembly);
            var carUsers = await carUsersService.GetAllAsync<CarUserViewModel>(carUserOne.CarId);

            Assert.Single(carUsers);
        }
    }
}
