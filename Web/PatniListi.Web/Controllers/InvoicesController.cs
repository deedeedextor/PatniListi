﻿namespace PatniListi.Web.Controllers
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
        private readonly ICarsService carsService;
        private readonly UserManager<ApplicationUser> userManager;

        public InvoicesController(IInvoicesService invoicesService, ICarsService carsService, UserManager<ApplicationUser> userManager)
        {
            this.invoicesService = invoicesService;
            this.carsService = carsService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> All(string id, int? pageNumber)
        {
            this.TempData["carId"] = id;

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

            viewModel.CreatedBy = this.userManager.GetUserAsync(this.User).Result?.FullName;
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
                AllFuelConsumption = this.carsService.GetCurrentFuelConsumptionByCarId(id),
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

            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            var user = await this.userManager.GetUserAsync(this.User);

            await this.invoicesService.CreateAsync(input.Number, input.Date, input.CarFuelType, input.Location, input.CurrentLiters, input.Price, input.Quantity, input.TotalPrice, user.Id, input.CreatedBy, input.CreatedBy);

            return this.RedirectToAction("All", "Invoices", new { id });
        }

        public async Task<IActionResult> Edit(string id)
        {
            var viewModel = await this.invoicesService.GetDetailsAsync<InvoiceEditViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            viewModel.CreatedBy = this.userManager.GetUserAsync(this.User).Result?.FullName;

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(InvoiceEditViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var viewModel = await this.invoicesService.GetDetailsAsync<InvoiceEditViewModel>(input.Id);

                return this.View(viewModel);
            }

            await this.invoicesService.EditAsync(input.Id, input.Number, input.Date, input.CarFuelType, input.Location, input.CurrentLiters, input.Price, input.Quantity, input.TotalPrice, input.UserId, input.CarId, input.CreatedBy, input.CreatedOn, input.ModifiedBy);

            return this.RedirectToAction("All", "Invoices", new { id = input.CarId });
        }

        public IActionResult ValidateFuelQuantity(double quantity, double currentLiters, int carTankCapacity)
        {
            if ((int)(currentLiters + quantity) > carTankCapacity)
            {
                return this.Json(data: "Наличното и зареденото количество гориво не трбва да надвишават капацитета на резервоара");
            }

            return this.Json(data: true);
        }

        public IActionResult ValidateInvoiceNumber(string number, string id)
        {
            bool exists = this.invoicesService.IsNumberExist(number);

            if (exists && id == null)
            {
                return this.Json(data: "Номерът на фактурата е зает.");
            }
            else if (exists && id != null)
            {
                if (number == this.invoicesService.GetInvoiceNumberById(id))
                {
                    return this.Json(data: true);
                }
                else
                {
                    return this.Json(data: "Номерът на фактурата е зает.");
                }
            }

            return this.Json(data: true);
        }
    }
}
