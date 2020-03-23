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
            this.TempData["id"] = id;

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
            return null;
        }

        public async Task<IActionResult> Create()
        {
            var id = this.TempData.Peek("id").ToString();

            var carFromDb = await this.carsService.GetDetailsAsync<CarDetailsViewModel>(id);
            var viewModel = new InvoiceInputViewModel
            {
                CarId = carFromDb.Id,
                CarFuelType = carFromDb.FuelType,
                CarModel = carFromDb.Model,
                CarCompanyId = carFromDb.CompanyId,
                AllDrivers = this.usersService.GetUsersByCar(id),
            };

            if (viewModel == null || viewModel.AllDrivers == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InvoiceInputViewModel input)
        {
            var id = this.TempData.Peek("id").ToString();

            if (!this.ModelState.IsValid)
            {
                input.AllDrivers = this.usersService.GetUsersByCar(input.CarId);
                return this.View(input);
            }

            await this.invoicesService.CreateAsync(input);

            return this.RedirectToAction("All", "Invoices", new { id });
        }

        public async Task<IActionResult> Edit(string id)
        {
            return null;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CarEditViewModel input)
        {
            return null;
        }

        public async Task<IActionResult> Delete(string id)
        {
            return null;
        }

        public async Task<IActionResult> ConfirmDelete(string id)
        {
            return null;
        }
    }
}
