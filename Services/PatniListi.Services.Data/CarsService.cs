namespace PatniListi.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using PatniListi.Data.Common.Repositories;
    using PatniListi.Data.Models;
    using PatniListi.Data.Models.Enums;
    using PatniListi.Services.Mapping;
    using PatniListi.Web.ViewModels.Administration.Cars;
    using PatniListi.Web.ViewModels.Administration.Users;

    public class CarsService : ICarsService
    {
        private readonly IDeletableEntityRepository<Car> carsRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUsersService usersService;

        public CarsService(IDeletableEntityRepository<Car> carsRepository, UserManager<ApplicationUser> userManager, IUsersService usersService)
        {
            this.carsRepository = carsRepository;
            this.userManager = userManager;
            this.usersService = usersService;
        }

        public async Task CreateAsync(CarInputViewModel input)
        {
            var user = await this.usersService.GetByNameAsync<UserViewModel>(input.FullName, input.CompanyId);

            var car = new Car
            {
                Model = input.Model,
                LicensePlate = input.LicensePlate,
                FuelType = (Fuel)Enum.Parse(typeof(Fuel), input.FuelType),
                StartKilometers = input.StartKilometers,
                AverageConsumption = input.AverageConsumption,
                TankCapacity = input.TankCapacity,
                InitialFuel = input.InitialFuel,
                CompanyId = input.CompanyId,
            };

            car.CarUsers.Add(new CarUser { UserId = user.Id, CarId = car.Id });

            await this.carsRepository.AddAsync(car);
            await this.carsRepository.SaveChangesAsync();
        }

        public Task EditAsync(CarInputViewModel input)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<T> GetAll<T>(string companyId)
        {
            return this.carsRepository
                .All()
                .Where(c => c.CompanyId == companyId)
                .OrderByDescending(c => c.CreatedOn)
                .To<T>();
        }

        public Task<T> GetDetailsAsync<T>(string id)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<SelectListItem> GetFuelType()
        {
            return new List<SelectListItem>
            {
                new SelectListItem { Value = "0", Text = "Бензин" },
                new SelectListItem { Value = "1", Text = "Дизел" },
                new SelectListItem { Value = "2", Text = "Газ" },
            };
        }
    }
}
