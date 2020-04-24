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
    using PatniListi.Web.ViewModels.Administration.Companies;
    using Xunit;

    public class CompaniesServiceTests
    {
        [Fact]
        public void GetUsersCountRetunsUsersCountByCompanyName()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var usersRepository = new EfDeletableEntityRepository<ApplicationUser>(new ApplicationDbContext(options.Options));
            var repository = new EfRepository<Company>(new ApplicationDbContext(options.Options));
            repository.AddAsync(new Company { Name = "Авангард ЕООД" });
            repository.AddAsync(new Company { Name = "ЕТ Саламандър" });
            repository.SaveChangesAsync();

            var companies = repository.AllAsNoTracking().ToList();

            for (int i = 0; i < companies.Count(); i++)
            {
                usersRepository.AddAsync(new ApplicationUser { UserName = $"mars{i}", Email = "mars@gmail.com", CompanyId = companies[i].Id, FullName = "Емилия Петрова", LastLoggingDate = DateTime.UtcNow });

                usersRepository.SaveChangesAsync();
            }

            var companiesService = new CompaniesService(repository);

            var companyCount = companiesService.GetUsersCount("Авангард ЕООД");

            Assert.Equal(1, companyCount);
        }

        [Fact]
        public async Task CreateAsyncCreatesCompany()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfRepository<Company>(new ApplicationDbContext(options.Options));

            var companiesService = new CompaniesService(repository);
            await companiesService.CreateAsync("Авангард ЕООД");
            await companiesService.CreateAsync("ЕТ Саламандър");

            var companyCount = repository.AllAsNoTracking().ToList().Count();

            Assert.Equal(2, companyCount);
        }

        [Fact]
        public async Task EditAsyncUpdatesExistingRoute()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfRepository<Company>(new ApplicationDbContext(options.Options));
            var company = new Company { Name = "Авангард ЕООД" };
            await repository.AddAsync(company);
            await repository.SaveChangesAsync();

            var companiesService = new CompaniesService(repository);

            company.Bulstat = "2638236483";
            company.VatNumber = $"BG{company.Bulstat}";
            company.PhoneNumber = "+ 359 999 000 000";

            await companiesService.EditAsync(company);

            var updatedCompany = repository.AllAsNoTracking().FirstOrDefault(c => c.Name == "Авангард ЕООД");

            Assert.Equal(company.Bulstat, updatedCompany.Bulstat);
            Assert.Equal(company.VatNumber, updatedCompany.VatNumber);
            Assert.Equal(company.PhoneNumber, updatedCompany.PhoneNumber);
        }

        [Fact]
        public async Task GetByNameAsyncReturnsCompanyId()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfRepository<Company>(new ApplicationDbContext(options.Options));
            await repository.AddAsync(new Company { Name = "Авангард ЕООД" });
            await repository.AddAsync(new Company { Name = "ЕТ Саламандър" });
            await repository.AddAsync(new Company { Name = "Тиесто Гранд" });
            await repository.SaveChangesAsync();

            var companiesService = new CompaniesService(repository);

            var companyIdDb = repository.AllAsNoTracking().Where(c => c.Name == "Авангард ЕООД").Select(c => c.Id).FirstOrDefault();

            var companyId = await companiesService.GetByNameAsync("Авангард ЕООД");

            Assert.Equal(companyIdDb, companyId);
        }

        [Fact]
        public async Task GetDetailsAsyncReturnsCompanyDetails()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfRepository<Company>(new ApplicationDbContext(options.Options));
            await repository.AddAsync(new Company { Name = "Авангард ЕООД" });
            await repository.AddAsync(new Company { Name = "ЕТ Саламандър" });
            await repository.AddAsync(new Company { Name = "Тиесто Гранд" });
            await repository.SaveChangesAsync();

            var companiesService = new CompaniesService(repository);

            var companyFromDb = repository.AllAsNoTracking().FirstOrDefault(c => c.Name == "Тиесто Гранд");

            AutoMapperConfig.RegisterMappings(typeof(DetailsCompanyViewModel).Assembly);
            var company = await companiesService.GetDetailsAsync<DetailsCompanyViewModel>(companyFromDb.Id);

            Assert.Equal(company.Name, companyFromDb.Name);
            Assert.Equal(company.Bulstat, companyFromDb.Bulstat);
            Assert.Equal(company.PhoneNumber, companyFromDb.PhoneNumber);
            Assert.Equal(company.Address, companyFromDb.Address);
        }

        [Fact]
        public void GetByIdReturnsCompany()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                         .UseInMemoryDatabase(Guid.NewGuid().ToString());

            var repository = new EfRepository<Company>(new ApplicationDbContext(options.Options));

            repository.AddAsync(new Company { Name = "Авангард ЕООД", Bulstat = "041835487509", VatNumber = $"BG{041835487509}", PhoneNumber = "+359 889 121 212", Address = "гр.Пловдив, ул.Марашев 5" });
            repository.AddAsync(new Company { Name = "ЕТ Саламандър", Bulstat = "63835487509", VatNumber = $"BG{041835487509}", PhoneNumber = "+359 889 211 212", Address = "гр.Пловдив, ул.Марашев 6" });
            repository.AddAsync(new Company { Name = "Тиесто Гранд", Bulstat = "041115487509", VatNumber = $"BG{041835487509}", PhoneNumber = "+359 889 121 211", Address = "гр.Пловдив, ул.Марашев 7" });
            repository.SaveChangesAsync();

            var routesService = new CompaniesService(repository);

            var routeFromDb = repository.AllAsNoTracking().FirstOrDefault(c => c.Name == "Тиесто Гранд");

            var route = routesService.GetById(routeFromDb.Id);

            Assert.Equal(routeFromDb.Id, route.Id);
            Assert.Equal(routeFromDb.Name, route.Name);
            Assert.Equal(routeFromDb.Bulstat, route.Bulstat);
            Assert.Equal(routeFromDb.Address, route.Address);
        }
    }
}
