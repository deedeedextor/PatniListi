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
    using PatniListi.Web.ViewModels.Administration.Users;
    using Xunit;

    public class UsersServiceTests
    {
        [Fact]
        public async Task DeleteAsyncReturnsTrueIfDriverExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var userOne = new ApplicationUser { UserName = "mars13", Email = "mars@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Емилия Петрова", LastLoggingDate = DateTime.UtcNow };
            var userTwo = new ApplicationUser { UserName = "rima32", Email = "petrov.12@gmail.com", CompanyId = "7480-32141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };
            await repository.AddAsync(userOne);
            await repository.AddAsync(userTwo);
            await repository.SaveChangesAsync();

            var usersService = new UsersService(repository);

            var isDeleted = await usersService.DeleteAsync(userOne.Id, "Петър Петров");
            var usersCount = repository.AllAsNoTracking().Count();

            Assert.Equal(1, usersCount);
            Assert.True(isDeleted);
        }

        [Fact]
        public async Task DeleteAsyncReturnsFalseIfDriverDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var userOne = new ApplicationUser { UserName = "mars13", Email = "mars@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Емилия Петрова", LastLoggingDate = DateTime.UtcNow };
            var userTwo = new ApplicationUser { UserName = "rima32", Email = "petrov.12@gmail.com", CompanyId = "7480-32141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };

            await repository.AddAsync(userOne);
            await repository.AddAsync(userTwo);
            await repository.SaveChangesAsync();

            var usersService = new UsersService(repository);

            var isDeleted = await usersService.DeleteAsync("7480-9100-3274983", "Петър Петров");
            var usersCount = repository.AllAsNoTracking().Count();

            Assert.Equal(2, usersCount);
            Assert.False(isDeleted);
        }

        [Fact]
        public void GetAllReturnsAllRoutesAsIQueryable()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var userOne = new ApplicationUser { UserName = "mars13", Email = "mars@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Емилия Петрова", LastLoggingDate = DateTime.UtcNow };
            var userTwo = new ApplicationUser { UserName = "rima32", Email = "petrov.12@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };
            var userThree = new ApplicationUser { UserName = "rima33", Email = "petrov.13@gmail.com", CompanyId = "7470-32141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };

            repository.AddAsync(userOne);
            repository.AddAsync(userTwo);
            repository.AddAsync(userThree);
            repository.SaveChangesAsync();

            var usersService = new UsersService(repository);

            AutoMapperConfig.RegisterMappings(typeof(UserViewModel).Assembly);
            var routes = usersService.GetAll<UserViewModel>("7480-9141-3274983").ToList();

            Assert.Equal(2, routes.Count());
        }

        [Fact]
        public void GetAllReturnsAllDriversAsCollectionOfSelectListItem()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var userOne = new ApplicationUser { UserName = "mars13", Email = "mars@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Емилия Петрова", LastLoggingDate = DateTime.UtcNow };
            var userTwo = new ApplicationUser { UserName = "rima32", Email = "petrov.12@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };
            var userThree = new ApplicationUser { UserName = "rima33", Email = "petrov.13@gmail.com", CompanyId = "7470-32141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };

            repository.AddAsync(userOne);
            repository.AddAsync(userTwo);
            repository.AddAsync(userThree);
            repository.SaveChangesAsync();

            var usersService = new UsersService(repository);

            var routes = usersService.GetAll("7480-9141-3274983");

            Assert.Equal(2, routes.Count());
        }

        [Fact]
        public void GetUsersByCarAsSelectedListItem()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var userOne = new ApplicationUser { UserName = "mars13", Email = "mars@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Емилия Петрова", LastLoggingDate = DateTime.UtcNow };
            userOne.CarUsers.Add(new CarUser { UserId = userOne.Id, CarId = "123-123-123" });
            userOne.CarUsers.Add(new CarUser { UserId = userOne.Id, CarId = "321-321-321" });
            var userTwo = new ApplicationUser { UserName = "rima32", Email = "petrov.12@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };
            userTwo.CarUsers.Add(new CarUser { UserId = userTwo.Id, CarId = "456-456-456" });
            var userThree = new ApplicationUser { UserName = "rima33", Email = "petrov.13@gmail.com", CompanyId = "7470-32141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };
            userThree.CarUsers.Add(new CarUser { UserId = userThree.Id, CarId = "123-123-123" });

            repository.AddAsync(userOne);
            repository.AddAsync(userTwo);
            repository.AddAsync(userThree);
            repository.SaveChangesAsync();

            var usersService = new UsersService(repository);

            var users = usersService.GetUsersByCar("123-123-123").Count();

            Assert.Equal(2, users);
        }

        [Fact]
        public async Task GetByIdAsyncReturnsDriver()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var userOne = new ApplicationUser { UserName = "mars13", Email = "mars@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Емилия Петрова", LastLoggingDate = DateTime.UtcNow };
            var userTwo = new ApplicationUser { UserName = "rima32", Email = "petrov.12@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };
            var userThree = new ApplicationUser { UserName = "rima33", Email = "petrov.13@gmail.com", CompanyId = "7470-32141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };

            await repository.AddAsync(userOne);
            await repository.AddAsync(userTwo);
            await repository.AddAsync(userThree);
            await repository.SaveChangesAsync();

            var usersService = new UsersService(repository);
            AutoMapperConfig.RegisterMappings(typeof(UserViewModel).Assembly);
            var user = await usersService.GetByIdAsync<UserViewModel>(userTwo.Id);

            Assert.Equal(userTwo.Id, user.Id);
            Assert.Equal(userTwo.UserName, user.Username);
            Assert.Equal(userTwo.Email, user.Email);
            Assert.Equal(userTwo.FullName, user.FullName);
        }

        [Fact]
        public async Task GetByNameAsyncReturnsDriver()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var userOne = new ApplicationUser { UserName = "mars13", Email = "mars@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Емилия Петрова", LastLoggingDate = DateTime.UtcNow };
            var userTwo = new ApplicationUser { UserName = "rima32", Email = "petrov.12@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };
            var userThree = new ApplicationUser { UserName = "rima33", Email = "petrov.13@gmail.com", CompanyId = "7470-32141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };

            await repository.AddAsync(userOne);
            await repository.AddAsync(userTwo);
            await repository.AddAsync(userThree);
            await repository.SaveChangesAsync();

            var usersService = new UsersService(repository);
            AutoMapperConfig.RegisterMappings(typeof(UserViewModel).Assembly);
            var user = await usersService.GetByNameAsync<UserViewModel>(userThree.FullName, userThree.CompanyId);

            Assert.Equal(userThree.Id, user.Id);
            Assert.Equal(userThree.UserName, user.Username);
            Assert.Equal(userThree.Email, user.Email);
            Assert.Equal(userThree.FullName, user.FullName);
        }

        [Fact]
        public async Task GetDetailsAsyncReturnsDriverInfo()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var userOne = new ApplicationUser { UserName = "mars13", Email = "mars@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Емилия Петрова", LastLoggingDate = DateTime.UtcNow };
            var carUserOne = new CarUser { UserId = userOne.Id, CarId = "123-123-123" };
            userOne.CarUsers.Add(carUserOne);
            var carUserTwo = new CarUser { UserId = userOne.Id, CarId = "321-321-321" };
            userOne.CarUsers.Add(carUserTwo);
            var userTwo = new ApplicationUser { UserName = "rima32", Email = "petrov.12@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };
            var carUserThree = new CarUser { UserId = userTwo.Id, CarId = "456-456-456" };
            userOne.CarUsers.Add(carUserThree);
            var userThree = new ApplicationUser { UserName = "rima33", Email = "petrov.13@gmail.com", CompanyId = "7470-32141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };
            var carUserFour = new CarUser { UserId = userThree.Id, CarId = "123-123-123" };
            userThree.CarUsers.Add(carUserFour);

            await repository.AddAsync(userOne);
            await repository.AddAsync(userTwo);
            await repository.AddAsync(userThree);
            await repository.SaveChangesAsync();

            var usersService = new UsersService(repository);
            AutoMapperConfig.RegisterMappings(typeof(UserDetailsViewModel).Assembly);
            var user = await usersService.GetDetailsAsync<UserDetailsViewModel>(userOne.Id);

            Assert.Equal(userOne.Id, user.Id);
            Assert.Equal(userOne.UserName, user.Username);
            Assert.Equal(userOne.Email, user.Email);
            Assert.Equal(userOne.FullName, user.FullName);
            Assert.Equal(userOne.FullName, user.FullName);
            Assert.Equal(userOne.CarUsers.Count(), user.AllCars.Count());
        }

        [Fact]
        public void IsUsernameInUseReturnsTrueWhenExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var userOne = new ApplicationUser { UserName = "mars13", Email = "mars@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Емилия Петрова", LastLoggingDate = DateTime.UtcNow };
            var userTwo = new ApplicationUser { UserName = "rima32", Email = "petrov.12@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };
            var userThree = new ApplicationUser { UserName = "rima33", Email = "petrov.13@gmail.com", CompanyId = "7470-32141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };

            repository.AddAsync(userOne);
            repository.AddAsync(userTwo);
            repository.AddAsync(userThree);
            repository.SaveChangesAsync();

            var usersService = new UsersService(repository);
            var exists = usersService.IsUsernameInUse(userOne.UserName);

            Assert.True(exists);
        }

        [Fact]
        public void IsUsernameInUseReturnsFalseWhenDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var userOne = new ApplicationUser { UserName = "mars13", Email = "mars@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Емилия Петрова", LastLoggingDate = DateTime.UtcNow };
            var userTwo = new ApplicationUser { UserName = "rima32", Email = "petrov.12@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };
            var userThree = new ApplicationUser { UserName = "rima33", Email = "petrov.13@gmail.com", CompanyId = "7470-32141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };

            repository.AddAsync(userOne);
            repository.AddAsync(userTwo);
            repository.AddAsync(userThree);
            repository.SaveChangesAsync();

            var usersService = new UsersService(repository);
            var exists = usersService.IsUsernameInUse("mars");

            Assert.False(exists);
        }

        [Fact]
        public void IsEmailInUseReturnsTrueWhenExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var userOne = new ApplicationUser { UserName = "mars13", Email = "mars@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Емилия Петрова", LastLoggingDate = DateTime.UtcNow };
            var userTwo = new ApplicationUser { UserName = "rima32", Email = "petrov.12@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };
            var userThree = new ApplicationUser { UserName = "rima33", Email = "petrov.13@gmail.com", CompanyId = "7470-32141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };

            repository.AddAsync(userOne);
            repository.AddAsync(userTwo);
            repository.AddAsync(userThree);
            repository.SaveChangesAsync();

            var usersService = new UsersService(repository);
            var exists = usersService.IsEmailInUse(userOne.Email);

            Assert.True(exists);
        }

        [Fact]
        public void IsEmailInUseReturnsFalseWhenDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var userOne = new ApplicationUser { UserName = "mars13", Email = "mars@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Емилия Петрова", LastLoggingDate = DateTime.UtcNow };
            var userTwo = new ApplicationUser { UserName = "rima32", Email = "petrov.12@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };
            var userThree = new ApplicationUser { UserName = "rima33", Email = "petrov.13@gmail.com", CompanyId = "7470-32141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };

            repository.AddAsync(userOne);
            repository.AddAsync(userTwo);
            repository.AddAsync(userThree);
            repository.SaveChangesAsync();

            var usersService = new UsersService(repository);
            var exists = usersService.IsUsernameInUse("mars123@abv.bg");

            Assert.False(exists);
        }

        [Fact]
        public void GetUsernameById()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var userOne = new ApplicationUser { UserName = "mars13", Email = "mars@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Емилия Петрова", LastLoggingDate = DateTime.UtcNow };
            var userTwo = new ApplicationUser { UserName = "rima32", Email = "petrov.12@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };
            var userThree = new ApplicationUser { UserName = "rima33", Email = "petrov.13@gmail.com", CompanyId = "7470-32141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };

            repository.AddAsync(userOne);
            repository.AddAsync(userTwo);
            repository.AddAsync(userThree);
            repository.SaveChangesAsync();

            var usersService = new UsersService(repository);
            var username = usersService.GetUsernameById(userThree.Id);

            Assert.Equal(userThree.UserName, username);
        }

        [Fact]
        public void GetEmailById()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));

            var userOne = new ApplicationUser { UserName = "mars13", Email = "mars@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Емилия Петрова", LastLoggingDate = DateTime.UtcNow };
            var userTwo = new ApplicationUser { UserName = "rima32", Email = "petrov.12@gmail.com", CompanyId = "7480-9141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };
            var userThree = new ApplicationUser { UserName = "rima33", Email = "petrov.13@gmail.com", CompanyId = "7470-32141-3274983", FullName = "Петър Петров", LastLoggingDate = DateTime.UtcNow };

            repository.AddAsync(userOne);
            repository.AddAsync(userTwo);
            repository.AddAsync(userThree);
            repository.SaveChangesAsync();

            var usersService = new UsersService(repository);
            var email = usersService.GetEmailById(userThree.Id);

            Assert.Equal(userThree.Email, email);
        }
    }
}
