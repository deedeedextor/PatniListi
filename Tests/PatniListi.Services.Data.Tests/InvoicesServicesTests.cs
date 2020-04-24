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
    using PatniListi.Web.ViewModels.Models.Invoices;
    using Xunit;

    public class InvoicesServicesTests
    {
        [Fact]
        public async Task CreateAsyncAddsCar()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Invoice>(new ApplicationDbContext(options.Options));
            var carsRepository = new EfDeletableEntityRepository<Car>(new ApplicationDbContext(options.Options));

            var carOne = new Car { Model = "Форд Фиеста", LicensePlate = "CO1212KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 4, InitialFuel = 10, StartKilometers = 234987 };
            await carsRepository.AddAsync(carOne);
            var carTwo = new Car { Model = "Форд Фиеста", LicensePlate = "CO4312KA", CompanyId = "72804eud-3464-hfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Бензин, TankCapacity = 55, AverageConsumption = 6, InitialFuel = 10, StartKilometers = 230444 };
            await carsRepository.AddAsync(carTwo);
            var carThree = new Car { Model = "Форд Фиеста 8", LicensePlate = "CO9812KA", CompanyId = "72804eudajhkhfvs-dasfa", FuelType = PatniListi.Data.Models.Enums.Fuel.Дизел, TankCapacity = 55, AverageConsumption = 5, InitialFuel = 10, StartKilometers = 234957 };
            await carsRepository.AddAsync(carThree);
            await carsRepository.SaveChangesAsync();

            var invoicesService = new InvoicesService(repository);

            var invoiceOne = new Invoice { Number = "43254325235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 10, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", Location = "София", Price = 2.09m, Quantity = 25.21, TotalPrice = 2.09m * 25.21m };
            var invoiceTwo = new Invoice { Number = "11254325235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 25, UserId = "331414153", CarId = "72804eudajhkhfvs-dasfa", Location = "София", Price = 2.07m, Quantity = 45.21, TotalPrice = 2.09m * 25.21m }; 
            var invoiceThree = new Invoice { Number = "43258825235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 20, UserId = "221414153", CarId = "72804eud-3464-hfvs-dasfa", Location = "Варна", Price = 2.06m, Quantity = 22.21, TotalPrice = 2.06m * 22.21m };

            await invoicesService.CreateAsync(invoiceOne);
            await invoicesService.CreateAsync(invoiceTwo);
            await invoicesService.CreateAsync(invoiceThree);

            var cars = repository.AllAsNoTracking().ToList().Count();

            Assert.Equal(3, cars);
        }

        [Fact]
        public async Task EditAsyncUpdatesCarInfo()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Invoice>(new ApplicationDbContext(options.Options));

            var invoicesService = new InvoicesService(repository);

            var invoiceOne = new Invoice { Number = "43254325235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 10, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", Location = "София", Price = 2.09m, Quantity = 25.21, TotalPrice = 2.09m * 25.21m };
            var invoiceTwo = new Invoice { Number = "11254325235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 25, UserId = "331414153", CarId = "72804eudajhkhfvs-dasfa", Location = "София", Price = 2.07m, Quantity = 45.21, TotalPrice = 2.09m * 25.21m };
            await repository.AddAsync(invoiceOne);
            await repository.AddAsync(invoiceTwo);
            await repository.SaveChangesAsync();

            invoiceOne.Number = "123456789";
            invoiceOne.Price = 2.12m;
            invoiceOne.TotalPrice = invoiceOne.Price * (decimal)invoiceOne.Quantity;

            await invoicesService.EditAsync(invoiceOne);

            var invoice = repository.AllAsNoTracking().FirstOrDefault(i => i.Id == invoiceOne.Id);

            Assert.Equal(invoiceOne.Number, invoice.Number);
            Assert.Equal(invoiceOne.Price, invoice.Price);
            Assert.Equal(invoiceOne.TotalPrice, invoice.TotalPrice);
        }

        [Fact]
        public void GetAllInvoicesAsQueryable()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Invoice>(new ApplicationDbContext(options.Options));

            var invoiceOne = new Invoice { Number = "43254325235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 10, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", Location = "София", Price = 2.09m, Quantity = 25.21, TotalPrice = 2.09m * 25.21m };
            var invoiceTwo = new Invoice { Number = "11254325235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 25, UserId = "331414153", CarId = "72804eudajhkhfvs-dasfa", Location = "София", Price = 2.07m, Quantity = 45.21, TotalPrice = 2.09m * 25.21m };

            repository.AddAsync(invoiceOne);
            repository.AddAsync(invoiceTwo);
            repository.SaveChangesAsync();

            var invoicesService = new InvoicesService(repository);

            AutoMapperConfig.RegisterMappings(typeof(InvoiceViewModel).Assembly);
            var invoices = invoicesService.GetAll<InvoiceViewModel>(invoiceTwo.CarId);

            Assert.Equal(2, invoices.Count());
        }

        [Fact]
        public void GetAllInvoicesForPeriodAsQueryable()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Invoice>(new ApplicationDbContext(options.Options));

            var invoiceOne = new Invoice { Number = "43254325235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 10, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", Location = "София", Price = 2.09m, Quantity = 25.21, TotalPrice = 2.09m * 25.21m };
            var invoiceTwo = new Invoice { Number = "11254325235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 25, UserId = "331414153", CarId = "72804eudajhkhfvs-dasfa", Location = "София", Price = 2.07m, Quantity = 45.21, TotalPrice = 2.09m * 25.21m };

            repository.AddAsync(invoiceOne);
            repository.AddAsync(invoiceTwo);
            repository.SaveChangesAsync();

            var invoicesService = new InvoicesService(repository);

            AutoMapperConfig.RegisterMappings(typeof(InvoiceViewModel).Assembly);
            var invoices = invoicesService.GetAllInvoicesForPeriod<InvoiceViewModel>(invoiceTwo.CarId, DateTime.UtcNow.AddDays(-10), DateTime.UtcNow);

            Assert.Equal(2, invoices.Count());
        }

        [Fact]
        public async Task GetDetailsAsync()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Invoice>(new ApplicationDbContext(options.Options));

            var invoiceOne = new Invoice { Number = "43254325235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 10, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", Location = "София", Price = 2.09m, Quantity = 25.21, TotalPrice = 2.09m * 25.21m };
            var invoiceTwo = new Invoice { Number = "11254325235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 25, UserId = "331414153", CarId = "72804eudajhkhfvs-dasfa", Location = "София", Price = 2.07m, Quantity = 45.21, TotalPrice = 2.09m * 25.21m };

            repository.AddAsync(invoiceOne);
            repository.AddAsync(invoiceTwo);
            repository.SaveChangesAsync();

            var invoicesService = new InvoicesService(repository);

            AutoMapperConfig.RegisterMappings(typeof(InvoiceDetailsViewModel).Assembly);
            var invoice = await invoicesService.GetDetailsAsync<InvoiceDetailsViewModel>(invoiceOne.Id);

            Assert.Equal(invoiceOne.Number, invoice.Number);
            Assert.Equal(invoiceOne.Price, invoice.Price);
            Assert.Equal(invoiceOne.TotalPrice, invoice.TotalPrice);
        }

        [Fact]
        public void GetInvoiceNumberById()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Invoice>(new ApplicationDbContext(options.Options));

            var invoiceOne = new Invoice { Number = "43254325235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 10, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", Location = "София", Price = 2.09m, Quantity = 25.21, TotalPrice = 2.09m * 25.21m };
            var invoiceTwo = new Invoice { Number = "11254325235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 25, UserId = "331414153", CarId = "72804eudajhkhfvs-dasfa", Location = "София", Price = 2.07m, Quantity = 45.21, TotalPrice = 2.09m * 25.21m };

            repository.AddAsync(invoiceOne);
            repository.AddAsync(invoiceTwo);
            repository.SaveChangesAsync();

            var invoicesService = new InvoicesService(repository);

            AutoMapperConfig.RegisterMappings(typeof(InvoiceDetailsViewModel).Assembly);
            var invoiceNumber = invoicesService.GetInvoiceNumberById(invoiceTwo.Id);

            Assert.Equal(invoiceTwo.Number, invoiceNumber);
        }

        [Fact]
        public void IsNumberExistReturnsTrueWhenAlreadyExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Invoice>(new ApplicationDbContext(options.Options));

            var invoiceOne = new Invoice { Number = "43254325235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 10, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", Location = "София", Price = 2.09m, Quantity = 25.21, TotalPrice = 2.09m * 25.21m };
            var invoiceTwo = new Invoice { Number = "11254325235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 25, UserId = "331414153", CarId = "72804eudajhkhfvs-dasfa", Location = "София", Price = 2.07m, Quantity = 45.21, TotalPrice = 2.09m * 25.21m };

            repository.AddAsync(invoiceOne);
            repository.AddAsync(invoiceTwo);
            repository.SaveChangesAsync();

            var invoicesService = new InvoicesService(repository);

            var exists = invoicesService.IsNumberExist(invoiceTwo.Number);

            Assert.True(exists);
        }

        [Fact]
        public void IsNumberExistReturnsFalseWhenDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Invoice>(new ApplicationDbContext(options.Options));

            var invoiceOne = new Invoice { Number = "43254325235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 10, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", Location = "София", Price = 2.09m, Quantity = 25.21, TotalPrice = 2.09m * 25.21m };
            var invoiceTwo = new Invoice { Number = "11254325235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 25, UserId = "331414153", CarId = "72804eudajhkhfvs-dasfa", Location = "София", Price = 2.07m, Quantity = 45.21, TotalPrice = 2.09m * 25.21m };

            repository.AddAsync(invoiceOne);
            repository.AddAsync(invoiceTwo);
            repository.SaveChangesAsync();

            var invoicesService = new InvoicesService(repository);
            var currentNumber = "74982403234";

            var exists = invoicesService.IsNumberExist(currentNumber);

            Assert.False(exists);
        }

        [Fact]
        public void GetByIdReturnsInvoice()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Invoice>(new ApplicationDbContext(options.Options));

            var invoiceOne = new Invoice { Number = "43254325235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 10, UserId = "341414153", CarId = "72804eudajhkhfvs-dasfa", Location = "София", Price = 2.09m, Quantity = 25.21, TotalPrice = 2.09m * 25.21m };
            var invoiceTwo = new Invoice { Number = "11254325235", Date = DateTime.UtcNow, FuelType = "Бензин", CurrentLiters = 25, UserId = "331414153", CarId = "72804eudajhkhfvs-dasfa", Location = "София", Price = 2.07m, Quantity = 45.21, TotalPrice = 2.09m * 25.21m };

            repository.AddAsync(invoiceOne);
            repository.AddAsync(invoiceTwo);
            repository.SaveChangesAsync();

            var invoicesService = new InvoicesService(repository);

            var invoice = invoicesService.GetById(invoiceOne.Id);

            Assert.Equal(invoiceOne.Number, invoice.Number);
            Assert.Equal(invoiceOne.Date, invoice.Date);
            Assert.Equal(invoiceOne.FuelType, invoice.FuelType);
            Assert.Equal(invoiceOne.CurrentLiters, invoice.CurrentLiters);
            Assert.Equal(invoiceOne.UserId, invoice.UserId);
            Assert.Equal(invoiceOne.CarId, invoice.CarId);
            Assert.Equal(invoiceOne.Location, invoice.Location);
            Assert.Equal(invoiceOne.Price, invoice.Price);
            Assert.Equal(invoiceOne.TotalPrice, invoice.TotalPrice);
        }
    }
}
