namespace PatniListi.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PatniListi.Data.Models;
    using PatniListi.Services.Data;
    using PatniListi.Web.ViewModels.Models.InvoiceReports;

    [Authorize]
    public class InvoiceReportsController : BaseController
    {
        private readonly ICarsService carsService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IInvoicesService invoicesService;

        public InvoiceReportsController(ICarsService carsService, UserManager<ApplicationUser> userManager, IInvoicesService invoicesService)
        {
            this.carsService = carsService;
            this.userManager = userManager;
            this.invoicesService = invoicesService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var viewModel = new InvoiceReportsIndexViewModel();

            if (this.User.IsInRole("Admin"))
            {
                viewModel.AllCars = this.carsService.GetAll(user.CompanyId);
            }
            else
            {
                viewModel.AllCars = this.carsService.GetAllCarsByUserId(user.Id, user.CompanyId);
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Index(InvoiceReportsIndexViewModel viewModel)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            if (this.User.IsInRole("Admin"))
            {
                viewModel.AllCars = this.carsService.GetAll(user.CompanyId);
            }
            else
            {
                viewModel.AllCars = this.carsService.GetAllCarsByUserId(user.Id, user.CompanyId);
            }

            viewModel.Invoices = this.invoicesService.GetAllInvoicesForPeriod<InvoiceReportsViewModel>(viewModel.CarId, viewModel.From, viewModel.To);

            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            return this.View("Index", viewModel);
        }

        public IActionResult ValidatePeriodBetweenDates(DateTime from, DateTime to)
        {
            var daysBetween = (to - from).TotalDays;

            if (daysBetween > 31)
            {
                return this.Json(data: "Избраният период не може да бъде по-голям от месец.");
            }

            return this.Json(data: true);
        }
    }
}
