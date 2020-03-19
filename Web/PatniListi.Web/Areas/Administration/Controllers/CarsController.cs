namespace PatniListi.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PatniListi.Common;
    using PatniListi.Data.Models;
    using PatniListi.Services.Data;
    using PatniListi.Web.Infrastructure;
    using PatniListi.Web.ViewModels.Administration.Cars;

    public class CarsController : AdministrationController
    {
        private readonly ICarsService carsService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUsersService usersService;

        public CarsController(ICarsService carsService, UserManager<ApplicationUser> userManager, IUsersService usersService)
        {
            this.carsService = carsService;
            this.userManager = userManager;
            this.usersService = usersService;
        }

        // GET: Cars
        public async Task<IActionResult> All(int? pageNumber)
        {
            var companyId = this.userManager.GetUserAsync(this.User).Result?.CompanyId;
            var cars = this.carsService
                .GetAll<CarViewModel>(companyId);

            return this.View(await PaginatedList<CarViewModel>.CreateAsync(cars, pageNumber ?? GlobalConstants.DefaultPageNumber, GlobalConstants.PageSize));
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(string id)
        {
            var viewModel = await this.carsService
                .GetDetailsAsync<CarDetailsViewModel>(id);

            return this.View(viewModel);
        }

        // GET: Cars/Create
        public IActionResult Create()
        {
            var companyId = this.userManager.GetUserAsync(this.User).Result?.CompanyId;

            var viewModel = new CarInputViewModel
            {
                AllDrivers = this.usersService.GetAll(companyId),
                AllTypes = this.carsService.GetFuelType(),
            };

            return this.View(viewModel);
        }

        // POST: Cars/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarInputViewModel input)
        {
            var companyId = input.CompanyId;

            if (!this.ModelState.IsValid)
            {
                input.AllTypes = this.carsService.GetFuelType();
                input.AllDrivers = this.usersService.GetAll(companyId);

                return this.View(input);
            }

            await this.carsService.CreateAsync(input);

            return this.RedirectToAction("All", "Cars");
        }

        // GET: Cars/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            var carToEdit = await this.carsService.GetDetailsAsync<CarEditViewModel>(id);

            carToEdit.AllDrivers = this.usersService.GetAll(carToEdit.CompanyId);
            carToEdit.AllTypes = this.carsService.GetFuelType();

            if (!this.ModelState.IsValid)
            {
                return this.NotFound();
            }

            return this.View(carToEdit);
        }

        // POST: Cars/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CarEditViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var carToEdit = await this.carsService.GetDetailsAsync<CarEditViewModel>(input.Id);

                carToEdit.AllDrivers = this.usersService.GetAll(carToEdit.CompanyId);
                carToEdit.AllTypes = this.carsService.GetFuelType();

                return this.View(carToEdit);
            }

            await this.carsService.EditAsync(input);

            return this.RedirectToAction("Details", "Cars", new { input.Id });
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            var viewModel = await this.carsService
                .GetDetailsAsync<CarDeleteViewModel>(id);

            if (!this.ModelState.IsValid)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        // POST: Cars/Delete/5
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            var deleted = await this.carsService.DeleteAsync(id);

            if (!deleted)
            {
                return this.NotFound();
            }

            return this.RedirectToAction("All", "Cars");
        }
    }
}