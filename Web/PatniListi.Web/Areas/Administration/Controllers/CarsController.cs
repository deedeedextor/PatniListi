namespace PatniListi.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PatniListi.Common;
    using PatniListi.Data.Models;
    using PatniListi.Data.Models.Enums;
    using PatniListi.Services.Data;
    using PatniListi.Services.Mapping;
    using PatniListi.Web.Infrastructure;
    using PatniListi.Web.ViewModels.Administration.Cars;

    public class CarsController : AdministrationController
    {
        private readonly ICarsService carsService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUsersService usersService;
        private readonly ICarUsersService carUsersService;

        public CarsController(ICarsService carsService, UserManager<ApplicationUser> userManager, IUsersService usersService, ICarUsersService carUsersService)
        {
            this.carsService = carsService;
            this.userManager = userManager;
            this.usersService = usersService;
            this.carUsersService = carUsersService;
        }

        public async Task<IActionResult> All(int? pageNumber)
        {
            var companyId = this.userManager.GetUserAsync(this.User).Result?.CompanyId;

            if (companyId == null)
            {
                return this.NotFound();
            }

            var cars = this.carsService
                .GetAll<CarViewModel>(companyId);

            return this.View(await PaginatedList<CarViewModel>.CreateAsync(cars, pageNumber ?? GlobalConstants.DefaultPageNumber, GlobalConstants.PageSize));
        }

        public async Task<IActionResult> Details(string id)
        {
            var viewModel = await this.carsService
                .GetDetailsAsync<CarDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        public IActionResult Create()
        {
            var companyId = this.userManager.GetUserAsync(this.User).Result?.CompanyId;

            var viewModel = new CarInputViewModel
            {
                AllTypes = this.carsService.GetFuelType(),
                AllDrivers = this.usersService.GetAll(companyId),
            };

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CarInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.AllTypes = this.carsService.GetFuelType();
                input.AllDrivers = this.usersService.GetAll(input.CompanyId);

                return this.View(input);
            }

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

            await this.carsService.CreateAsync(car);
            await this.carUsersService.UpdateAsync(car.Id, car.CompanyId, input.FullName);

            return this.RedirectToAction("All", "Cars");
        }

        public async Task<IActionResult> Edit(string id)
        {
            var carToEdit = await this.carsService.GetDetailsAsync<CarEditViewModel>(id);

            if (carToEdit == null)
            {
                return this.NotFound();
            }

            carToEdit.AllTypes = this.carsService.GetFuelType();
            carToEdit.AllDrivers = this.usersService.GetAll(carToEdit.CompanyId);

            return this.View(carToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CarEditViewModel input)
        {
            var currentUserFullname = this.userManager.GetUserAsync(this.User).Result?.FullName;

            if (!this.ModelState.IsValid)
            {
                var carToEdit = await this.carsService.GetDetailsAsync<CarEditViewModel>(input.Id);

                carToEdit.AllTypes = this.carsService.GetFuelType();
                carToEdit.AllDrivers = this.usersService.GetAll(carToEdit.CompanyId);

                return this.View(carToEdit);
            }

            var car = this.carsService.GetById(input.Id);
            car.Model = input.Model;
            car.LicensePlate = input.LicensePlate;
            car.FuelType = (Fuel)Enum.Parse(typeof(Fuel), input.FuelType);
            car.StartKilometers = input.StartKilometers;
            car.AverageConsumption = input.AverageConsumption;
            car.TankCapacity = input.TankCapacity;
            car.InitialFuel = input.InitialFuel;
            car.CompanyId = input.CompanyId;
            car.CreatedOn = input.CreatedOn;
            car.ModifiedBy = input.ModifiedBy;

            await this.carsService.EditAsync(car, currentUserFullname);
            await this.carUsersService.UpdateAsync(input.Id, input.CompanyId, input.FullName);

            return this.RedirectToAction("All", "Cars");
        }

        public async Task<IActionResult> Delete(string id)
        {
            var viewModel = await this.carsService
                .GetDetailsAsync<CarDeleteViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> ConfirmDelete(string id)
        {
            var currentUserFullname = this.userManager.GetUserAsync(this.User).Result?.FullName;

            var deleted = await this.carsService.DeleteAsync(id, currentUserFullname);

            if (!deleted)
            {
                return this.NotFound();
            }

            await this.carUsersService.SetIsDeletedAsync(id, currentUserFullname);
            return this.RedirectToAction("All", "Cars");
        }
    }
}
