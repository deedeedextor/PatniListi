namespace PatniListi.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

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
                AllDrivers = this.usersService.GetAll(companyId),
                AllTypes = this.carsService.GetFuelType(),
            };

            if (companyId == null || viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

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

        public async Task<IActionResult> Edit(string id)
        {
            var carToEdit = await this.carsService.GetDetailsAsync<CarEditViewModel>(id);

            if (carToEdit == null)
            {
                return this.NotFound();
            }

            carToEdit.AllDrivers = this.usersService.GetAll(carToEdit.CompanyId);
            carToEdit.AllTypes = this.carsService.GetFuelType();

            return this.View(carToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CarEditViewModel input)
        {
            var currentUserFullname = this.userManager.GetUserAsync(this.User).Result?.FullName;

            if (!this.ModelState.IsValid)
            {
                var carToEdit = await this.carsService.GetDetailsAsync<CarEditViewModel>(input.Id);

                carToEdit.AllDrivers = this.usersService.GetAll(carToEdit.CompanyId);
                carToEdit.AllTypes = this.carsService.GetFuelType();

                return this.View(carToEdit);
            }

            await this.carsService.EditAsync(input, currentUserFullname);

            return this.RedirectToAction("Details", "Cars", new { input.Id });
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

            return this.RedirectToAction("All", "Cars");
        }
    }
}