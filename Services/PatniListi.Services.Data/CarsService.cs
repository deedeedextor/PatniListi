namespace PatniListi.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using PatniListi.Data.Common.Repositories;
    using PatniListi.Data.Models;
    using PatniListi.Data.Models.Enums;
    using PatniListi.Services.Mapping;
    using PatniListi.Web.ViewModels.Administration.Cars;

    public class CarsService : ICarsService
    {
        private readonly IDeletableEntityRepository<Car> carsRepository;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUsersService usersService;
        private readonly ICarUsersService carUsersService;

        public CarsService(IDeletableEntityRepository<Car> carsRepository, UserManager<ApplicationUser> userManager, IUsersService usersService, ICarUsersService carUsersService)
        {
            this.carsRepository = carsRepository;
            this.userManager = userManager;
            this.usersService = usersService;
            this.carUsersService = carUsersService;
        }

        public async Task CreateAsync(CarInputViewModel input)
        {
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

            await this.carsRepository.AddAsync(car);
            await this.carUsersService.UpdateAsync(car.Id, car.CompanyId, input.FullName);

            await this.carsRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(string id, string fullName)
        {
            var car = await this.carsRepository
                .All()
                .Where(c => c.Id == id)
                .FirstOrDefaultAsync();

            if (car == null)
            {
                return false;
            }

            this.carsRepository.Delete(car);
            await this.carUsersService.SetIsDeletedAsync(car.Id, fullName);
            await this.carsRepository.SaveChangesAsync();

            return true;
        }

        public async Task EditAsync(CarEditViewModel input, string fullName)
        {
            var car = new Car
            {
                Id = input.Id,
                Model = input.Model,
                LicensePlate = input.LicensePlate,
                FuelType = (Fuel)Enum.Parse(typeof(Fuel), input.FuelType),
                StartKilometers = input.StartKilometers,
                AverageConsumption = input.AverageConsumption,
                TankCapacity = input.TankCapacity,
                InitialFuel = input.InitialFuel,
                CompanyId = input.CompanyId,
                CreatedOn = input.CreatedOn,
                ModifiedBy = fullName,
            };

            await this.carUsersService.UpdateAsync(input.Id, input.CompanyId, input.FullName);

            this.carsRepository.Update(car);
            await this.carsRepository.SaveChangesAsync();
        }

        public IQueryable<T> GetAll<T>(string companyId)
        {
            return this.carsRepository
                .AllAsNoTracking()
                .Where(c => c.CompanyId == companyId)
                .OrderByDescending(c => c.CreatedOn)
                .To<T>();
        }

        public IQueryable<T> GetCarsByUser<T>(string userId, string companyId)
        {
            return this.carsRepository
                   .AllAsNoTracking()
                   .Where(c => c.CompanyId == companyId && c.CarUsers.Any(cu => cu.UserId == userId))
                   .To<T>();
        }

        public IEnumerable<SelectListItem> GetAll(string companyId)
        {
            return this.carsRepository
                   .All()
                   .Where(c => c.CompanyId == companyId)
                   .Select(c => new SelectListItem
                   {
                       Value = c.Model,
                       Text = c.Model,
                   })
                   .ToList();
        }

        public double GetCurrentLitresByCarId(string id)
        {
            var litres = this.carsRepository
                         .AllAsNoTracking()
                         .Where(c => c.Id == id)
                         .Select(i => i.Invoices.Sum(i => i.Quantity))
                         .SingleOrDefault();

            return litres;
        }

        public double GetCurrentTravelledDistanceByCarId(string id)
        {
            var travelledDistance = this.carsRepository
                            .AllAsNoTracking()
                            .Where(c => c.Id == id)
                            .Select(i => i.TransportWorkTickets.Sum(i => i.TravelledDistance))
                            .SingleOrDefault();

            return travelledDistance;
        }

        public double GetCurrentFuelConsumptionByCarId(string id)
        {
            var residue = this.carsRepository
                          .AllAsNoTracking()
                          .Where(c => c.Id == id)
                          .Select(i => i.TransportWorkTickets.Sum(i => i.FuelConsumption))
                          .SingleOrDefault();

            return residue;
        }

        public IEnumerable<SelectListItem> GetAllUsersForCar(string id)
        {
            return this.carsRepository
                   .All()
                   .Include(c => c.CarUsers)
                   .Where(c => c.Id == id)
                   .Select(c => new SelectListItem
                   {
                       Value = c.Model,
                       Text = c.Model,
                   })
                   .ToList();
        }

        public async Task<T> GetByIdAsync<T>(string carId)
        {
            return await this.carsRepository
                .All()
                .Where(c => c.Id == carId)
                .To<T>()
                .SingleOrDefaultAsync();
        }

        public async Task<T> GetDetailsAsync<T>(string id)
        {
            var viewModel = await this.carsRepository
                .All()
                .Include(c => c.CarUsers)
                .Where(c => c.Id == id)
                .To<T>()
                .SingleOrDefaultAsync();

            return viewModel;
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
