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
    using PatniListi.Web.ViewModels.Models.Routes;
    using Xunit;

    public class RoutesServiceTests
    {
        [Fact]
        public async Task CreateAsyncAddRouteToDbIfDoesNotExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Route>(new ApplicationDbContext(options.Options));
            var routesService = new RoutesService(repository);
            await routesService.CreateAsync("Видин", "Добрич", 189);

            var routeCount = repository.AllAsNoTracking().ToList().Count();

            Assert.Equal(1, routeCount);
        }

        [Fact]
        public async Task CreateAsyncDoNotAddRouteIfExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Route>(new ApplicationDbContext(options.Options));
            var routesService = new RoutesService(repository);
            await routesService.CreateAsync("Видин", "Добрич", 189);
            await routesService.CreateAsync("Видин", "Добрич", 189);

            var routeCount = repository.All().ToList().Count();

            Assert.Equal(1, routeCount);
        }

        [Fact]
        public async Task EditAsyncUpdatesExistingRoute()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Route>(new ApplicationDbContext(options.Options));

            var route = new Route
            {
                StartPoint = "Видин",
                EndPoint = "Добрич",
                Distance = 189,
            };

            await repository.AddAsync(route);
            await repository.SaveChangesAsync();
            var routesService = new RoutesService(repository);

            route.StartPoint = "Видин";
            route.EndPoint = "Добрич";
            route.Distance = 200;

            await routesService.EditAsync(route);

            var updatedRoute = repository.AllAsNoTracking().FirstOrDefault(r => r.Id == route.Id);

            Assert.Equal(route.StartPoint, updatedRoute.StartPoint);
            Assert.Equal(route.EndPoint, updatedRoute.EndPoint);
            Assert.Equal(route.Distance, updatedRoute.Distance);
        }

        [Fact]
        public void GetAllReturnsAllRoutesAsIQueryable()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Route>(new ApplicationDbContext(options.Options));

            repository.AddAsync(new Route { StartPoint = "София", EndPoint = "Пловдив", Distance = 225 });
            repository.AddAsync(new Route { StartPoint = "София", EndPoint = "Хасково", Distance = 267 });
            repository.AddAsync(new Route { StartPoint = "София", EndPoint = "Варна", Distance = 441 });

            repository.SaveChangesAsync();

            var routesService = new RoutesService(repository);
            AutoMapperConfig.RegisterMappings(typeof(RouteViewModel).Assembly);
            var routes = routesService.GetAll<RouteViewModel>();

            Assert.Equal(3, routes.Count());
        }

        [Fact]
        public void GetAllReturnsAllRoutesAsCollectionOfSelectListItem()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Route>(new ApplicationDbContext(options.Options));

            repository.AddAsync(new Route { StartPoint = "София", EndPoint = "Пловдив", Distance = 225 });
            repository.AddAsync(new Route { StartPoint = "София", EndPoint = "Хасково", Distance = 267 });
            repository.AddAsync(new Route { StartPoint = "София", EndPoint = "Варна", Distance = 441 });

            repository.SaveChangesAsync();

            var routesService = new RoutesService(repository);
            var routes = routesService.GetAll();

            Assert.Equal(3, routes.Count());
        }

        [Fact]
        public async Task GetByIdAsyncReturnsRoute()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Route>(new ApplicationDbContext(options.Options));

            await repository.AddAsync(new Route { StartPoint = "София", EndPoint = "Пловдив", Distance = 225 });
            await repository.SaveChangesAsync();
            var routesService = new RoutesService(repository);

            var route = repository.AllAsNoTracking().FirstOrDefault();
            AutoMapperConfig.RegisterMappings(typeof(RouteViewModel).Assembly);
            var currentRoute = await routesService.GetByIdAsync<RouteViewModel>(route.Id);

            Assert.Equal(route.StartPoint, currentRoute.StartPoint);
            Assert.Equal(route.EndPoint, currentRoute.EndPoint);
            Assert.Equal(route.Distance, currentRoute.Distance);
        }

        [Fact]
        public async Task GetDetailsAsyncReturnsRoute()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Route>(new ApplicationDbContext(options.Options));

            await repository.AddAsync(new Route { StartPoint = "София", EndPoint = "Пловдив", Distance = 225 });
            await repository.AddAsync(new Route { StartPoint = "София", EndPoint = "Хасково", Distance = 267 });
            await repository.SaveChangesAsync();
            var routesService = new RoutesService(repository);

            var route = repository.AllAsNoTracking().FirstOrDefault(r => r.StartPoint == "София" && r.EndPoint == "Пловдив");

            AutoMapperConfig.RegisterMappings(typeof(RouteDetailsViewModel).Assembly);
            var currentRoute = await routesService.GetByIdAsync<RouteDetailsViewModel>(route.Id);

            Assert.Equal(route.StartPoint, currentRoute.StartPoint);
            Assert.Equal(route.EndPoint, currentRoute.EndPoint);
            Assert.Equal(route.Distance, currentRoute.Distance);
        }

        [Fact]
        public void IsExistsReturnsTrueWhenRouteExists()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Route>(new ApplicationDbContext(options.Options));

            repository.AddAsync(new Route { StartPoint = "София", EndPoint = "Пловдив", Distance = 225 });
            repository.AddAsync(new Route { StartPoint = "София", EndPoint = "Хасково", Distance = 267 });
            repository.SaveChangesAsync();
            var routesService = new RoutesService(repository);

            var route = repository.AllAsNoTracking().FirstOrDefault(r => r.StartPoint == "София" && r.EndPoint == "Пловдив");
            var exists = routesService.IsExists(route.StartPoint, route.EndPoint);

            Assert.True(exists);
        }

        [Fact]
        public void IsExistsReturnsFalseWhenRouteDoesNotExist()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Route>(new ApplicationDbContext(options.Options));

            repository.AddAsync(new Route { StartPoint = "София", EndPoint = "Пловдив", Distance = 225 });
            repository.AddAsync(new Route { StartPoint = "София", EndPoint = "Хасково", Distance = 267 });
            repository.SaveChangesAsync();
            var routesService = new RoutesService(repository);

            var exists = routesService.IsExists("Стара Загора", "София");

            Assert.False(exists);
        }

        [Fact]
        public void GetByIdReturnsRoute()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfDeletableEntityRepository<Route>(new ApplicationDbContext(options.Options));

            repository.AddAsync(new Route { StartPoint = "София", EndPoint = "Пловдив", Distance = 225 });
            repository.AddAsync(new Route { StartPoint = "Варна", EndPoint = "Хасково", Distance = 341 });
            repository.SaveChangesAsync();
            var routesService = new RoutesService(repository);

            var routeFromDb = repository.AllAsNoTracking().FirstOrDefault(r => r.StartPoint == "Варна" && r.EndPoint == "Хасково");

            var route = routesService.GetById(routeFromDb.Id);

            Assert.Equal(routeFromDb.Id, route.Id);
            Assert.Equal(routeFromDb.StartPoint, route.StartPoint);
            Assert.Equal(routeFromDb.EndPoint, route.EndPoint);
            Assert.Equal(routeFromDb.Distance, route.Distance);
        }
    }
}
