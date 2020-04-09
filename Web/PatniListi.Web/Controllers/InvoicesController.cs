namespace PatniListi.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PatniListi.Common;
    using PatniListi.Data.Models;
    using PatniListi.Services.Data;
    using PatniListi.Web.Infrastructure;
    using PatniListi.Web.ViewModels.Models.Cars;
    using PatniListi.Web.ViewModels.Models.Invoices;

    [Authorize]
    public class InvoicesController : BaseController
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
                AllLitres = this.carsService.GetCurrentLitresByCarId(id),
                AllFuelConsumption = this.carsService.GetCurrentTravelledDistanceByCarId(id),
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
            var id = this.TempData.Peek("carId").ToString();

            var user = await this.userManager.GetUserAsync(this.User);

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.invoicesService.CreateAsync(input.Number, input.Date, input.Location, input.CurrentLiters, input.Price, input.Quantity, user.FullName, input.CarId, input.CarCompanyId, input.CreatedBy, input.CarFuelType, input.TotalPrice);

            return this.RedirectToAction("All", "Invoices", new { id });
        }

        public async Task<IActionResult> Edit(string id)
        {
            var viewModel = await this.invoicesService.GetDetailsAsync<InvoiceEditViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(InvoiceEditViewModel input)
        {
            var currentUserFullname = this.userManager.GetUserAsync(this.User).Result?.FullName;

            if (!this.ModelState.IsValid)
            {
                var viewModel = await this.invoicesService.GetDetailsAsync<InvoiceEditViewModel>(input.Id);

                return this.View(viewModel);
            }

            await this.invoicesService.EditAsync(input.Id, input.Number, input.Date, input.Location, input.CurrentLiters, input.Price, input.Quantity, input.ApplicationUserFullName, input.CarId, input.CarCompanyId, input.CreatedBy, input.CreatedOn, input.CarFuelType, input.TotalPrice, currentUserFullname);
            return this.RedirectToAction("All", "Invoices", new { id = input.CarId });
        }

        public IActionResult ValidateQuantity(double quantity, double currentLiters, int carTankCapacity)
        {
            if ((int)(currentLiters + quantity) > carTankCapacity)
            {
                return this.Json(data: "Наличното и зареденото количество гориво не трбва да надвишават капацитета на резервоара");
            }

            return this.Json(data: true);
        }
    }
}
