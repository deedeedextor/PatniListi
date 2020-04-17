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

    [Authorize]
    public class CarsController : BaseController
    {
        private readonly ICarsService carsService;
        private readonly UserManager<ApplicationUser> userManager;

        public CarsController(ICarsService carsService, UserManager<ApplicationUser> userManager)
        {
            this.carsService = carsService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> All(int? pageNumber)
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var cars = this.carsService
                .GetCarsByUser<CarViewModel>(user.Id, user.CompanyId);

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
    }
}
