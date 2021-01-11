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

            var car = await this.carsService.CreateAsync(input.Model, input.LicensePlate, input.FuelType, input.StartKilometers, input.AverageConsumption, input.TankCapacity, input.InitialFuel, input.CompanyId);
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

            await this.carsService.EditAsync(input.Id, input.Model, input.LicensePlate, input.FuelType, input.StartKilometers, input.AverageConsumption, input.TankCapacity, input.InitialFuel, input.CompanyId, input.CreatedOn, input.ModifiedBy, currentUserFullname);
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
            var user = this.userManager.GetUserAsync(this.User).Result;

            var deleted = await this.carsService.DeleteAsync(id, user.CompanyId);

            if (!deleted)
            {
                return this.NotFound();
            }

            await this.carUsersService.SetIsDeletedAsync(id, user.FullName);
            return this.RedirectToAction("All", "Cars");
        }
    }
}
