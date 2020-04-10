namespace PatniListi.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using PatniListi.Data.Models;
    using PatniListi.Services.Data;
    using PatniListi.Web.ViewModels.Models.TransportWorkTicketReports;

    [Authorize]
    public class TransportWorkTicketReportsController : BaseController
    {
        private readonly ICarsService carsService;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITransportWorkTicketsService transportWorkTicketsService;

        public TransportWorkTicketReportsController(ICarsService carsService, UserManager<ApplicationUser> userManager, ITransportWorkTicketsService transportWorkTicketsService)
        {
            this.carsService = carsService;
            this.userManager = userManager;
            this.transportWorkTicketsService = transportWorkTicketsService;
        }

        public async Task<IActionResult> Index()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var viewModel = new TransportWorkTicketReportsIndexViewModel();

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
        public async Task<IActionResult> Index(TransportWorkTicketReportsIndexViewModel viewModel)
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

            viewModel.TransportWorkTickets = this.transportWorkTicketsService.GetAllTransportWorkTicketsForPeriod<TransportWorkTicketReportsViewModel>(viewModel.CarId, viewModel.From, viewModel.To);

            if (!this.ModelState.IsValid)
            {
                return this.View(viewModel);
            }

            return this.View("Index", viewModel);
        }
    }
}
