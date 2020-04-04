namespace PatniListi.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PatniListi.Common;
    using PatniListi.Services.Data;
    using PatniListi.Web.Infrastructure;
    using PatniListi.Web.ViewModels.Models.Routes;

    [Authorize]
    public class RoutesController : BaseController
    {
        private readonly IRoutesService routesService;

        public RoutesController(IRoutesService routesService)
        {
            this.routesService = routesService;
        }

        public async Task<IActionResult> All(int? pageNumber)
        {
            var invoices = this.routesService
                .GetAll<RouteViewModel>();

            return this.View(await PaginatedList<RouteViewModel>.CreateAsync(invoices, pageNumber ?? GlobalConstants.DefaultPageNumber, GlobalConstants.PageSize));
        }

        public IActionResult Create()
        {
            this.ViewBag.returnUrl = this.Request.Headers["Referer"].ToString();
            return this.View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RouteInputViewModel input, string returnUrl)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.routesService.CreateAsync(input.StartPoint, input.EndPoint, input.Distance);

            return this.Redirect(returnUrl);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var viewModel = await this.routesService.GetDetailsAsync<RouteEditViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(RouteEditViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.routesService.EditAsync(input.Id, input.StartPoint, input.EndPoint, input.Distance, input.CreatedOn);
            return this.RedirectToAction("All", "Routes");
        }
    }
}
