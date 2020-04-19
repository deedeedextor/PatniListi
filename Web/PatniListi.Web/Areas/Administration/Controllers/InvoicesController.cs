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
    using PatniListi.Web.ViewModels.Administration.Invoices;

    public class InvoicesController : AdministrationController
    {
        private readonly IInvoicesService invoicesService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IUsersService usersService;
        private readonly ICarsService carsService;

        public InvoicesController(IInvoicesService invoicesService, UserManager<ApplicationUser> userManager, IUsersService usersService, ICarsService carsService)
        {
            this.invoicesService = invoicesService;
            this.userManager = userManager;
            this.usersService = usersService;
            this.carsService = carsService;
        }

        public async Task<IActionResult> All(string id, int? pageNumber)
        {
            this.TempData["carId"] = id;

            if (id == null)
            {
                return this.NotFound();
            }

            var invoices = this.invoicesService
                .GetAll<InvoiceViewModel>(id);

            return this.View(await PaginatedList<InvoiceViewModel>.CreateAsync(invoices, pageNumber ?? GlobalConstants.DefaultPageNumber, GlobalConstants.PageSize));
        }

        public async Task<IActionResult> Details(string id)
        {
            var viewModel = await this.invoicesService
                .GetDetailsAsync<InvoiceDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var id = this.TempData.Peek("carId").ToString();

            var carFromDb = await this.carsService.GetDetailsAsync<CarDetailsViewModel>(id);
            var viewModel = new InvoiceInputViewModel
            {
                CarId = carFromDb.Id,
                CarFuelType = carFromDb.FuelType,
                CarModel = carFromDb.Model,
                CarCompanyId = carFromDb.CompanyId,
                CarInitialFuel = carFromDb.InitialFuel,
                CarTankCapacity = carFromDb.TankCapacity,
                AllDrivers = this.usersService.GetAll(carFromDb.CompanyId),
                AllLitres = this.carsService.GetCurrentLitresByCarId(id),
                AllFuelConsumption = this.carsService.GetCurrentFuelConsumptionByCarId(carFromDb.Id),
            };

            viewModel.CurrentLiters = viewModel.Liters;

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(InvoiceInputViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                input.AllDrivers = this.usersService.GetUsersByCar(input.CarId);
                return this.View(input);
            }

            await this.invoicesService.CreateAsync(input.Number, input.Date, input.Location, input.CurrentLiters, input.Price, input.Quantity, input.FullName, input.CarId, input.CarCompanyId, input.CreatedBy, input.CarFuelType, input.TotalPrice);

            return this.RedirectToAction("All", "Invoices", new { id = input.CarId });
        }

        public async Task<IActionResult> Edit(string id)
        {
            var viewModel = await this.invoicesService.GetDetailsAsync<InvoiceEditViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            viewModel.AllDrivers = this.usersService.GetAll(viewModel.CarCompanyId);

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(InvoiceEditViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var viewModel = await this.invoicesService.GetDetailsAsync<InvoiceEditViewModel>(input.Id);
                viewModel.AllDrivers = this.usersService.GetAll(viewModel.CarCompanyId);

                return this.View(viewModel);
            }

            await this.invoicesService.EditAsync(input.Id, input.Number, input.Date, input.Location, input.CurrentLiters, input.Price, input.Quantity, input.ApplicationUserFullName, input.CarId, input.CarCompanyId, input.CreatedBy, input.CreatedOn, input.ModifiedBy, input.CarFuelType, input.TotalPrice);

            return this.RedirectToAction("All", "Invoices", new { id = input.CarId });
        }
    }
}
