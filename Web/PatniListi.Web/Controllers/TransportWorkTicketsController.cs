namespace PatniListi.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PatniListi.Common;
    using PatniListi.Services.Data;
    using PatniListi.Web.Infrastructure;
    using PatniListi.Web.ViewModels.Models.Cars;
    using PatniListi.Web.ViewModels.Models.Routes;
    using PatniListi.Web.ViewModels.Models.TransportWorkTickets;

    [Authorize]
    public class TransportWorkTicketsController : BaseController
    {
        private readonly ITransportWorkTicketsService transportWorkTicketsService;
        private readonly ICarsService carsService;
        private readonly IUsersService usersService;
        private readonly IRoutesService routesService;

        public TransportWorkTicketsController(ITransportWorkTicketsService transportWorkTicketsService, ICarsService carsService, IUsersService usersService, IRoutesService routesService)
        {
            this.transportWorkTicketsService = transportWorkTicketsService;
            this.carsService = carsService;
            this.usersService = usersService;
            this.routesService = routesService;
        }

        public async Task<IActionResult> All(string id, int? pageNumber)
        {
            this.TempData["carId"] = id;

            var workTickets = this.transportWorkTicketsService
                .GetAll<TransportWorkTicketViewModel>(id);

            return this.View(await PaginatedList<TransportWorkTicketViewModel>.CreateAsync(workTickets, pageNumber ?? GlobalConstants.DefaultPageNumber, GlobalConstants.PageSize));
        }

        public async Task<IActionResult> Create()
        {
            var id = this.TempData.Peek("carId").ToString();

            var carFromDb = await this.carsService.GetDetailsAsync<CarDetailsViewModel>(id);
            var viewModel = new TransportWorkTicketInputViewModel
            {
                CarId = carFromDb.Id,
                CarFuelType = carFromDb.FuelType,
                CarModel = carFromDb.Model,
                CarCompanyId = carFromDb.CompanyId,
                CarAverageConsumption = carFromDb.AverageConsumption,
                CarInitialFuel = carFromDb.InitialFuel,
                CarLicensePlate = carFromDb.LicensePlate,
                CarStartKilometers = carFromDb.StartKilometers,
                CarTankCapacity = carFromDb.TankCapacity,
                AllLiters = this.carsService.GetCurrentLitresByCarId(carFromDb.Id),
                AllTravelledDistance = this.carsService.GetCurrentTravelledDistanceByCarId(carFromDb.Id),
                AllFuelConsumption = this.carsService.GetCurrentFuelConsumptionByCarId(carFromDb.Id),
                AllRoutes = this.routesService.GetAll(),
            };

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(TransportWorkTicketInputViewModel input)
        {
            var id = this.TempData.Peek("carId").ToString();

            if (!this.ModelState.IsValid)
            {
                input.AllLiters = this.carsService.GetCurrentLitresByCarId(input.CarId);
                input.AllTravelledDistance = this.carsService.GetCurrentTravelledDistanceByCarId(input.CarId);
                input.AllFuelConsumption = this.carsService.GetCurrentFuelConsumptionByCarId(input.CarId);
                input.AllRoutes = this.routesService.GetAll();

                return this.View(input);
            }

            await this.transportWorkTicketsService.CreateAsync(input.Date, input.ApplicationUserFullName, input.CarId, input.CarCompanyId, input.CreatedBy, input.Route, input.StartKilometers, input.EndKilometers, input.FuelConsumption, input.Residue, input.FuelAvailability, input.TravelledDistance);

            return this.RedirectToAction("All", "TransportWorkTickets", new { id });
        }

        public async Task<IActionResult> Details(string id)
        {
            var viewModel = await this.transportWorkTicketsService
                .GetDetailsAsync<TransportWorkTicketDetailsViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var viewModel = await this.transportWorkTicketsService.GetDetailsAsync<TransportWortkTicketEditViewModel>(id);

            if (viewModel == null)
            {
                return this.NotFound();
            }

            viewModel.AllDrivers = this.usersService.GetUsersByCar(viewModel.CarId);
            viewModel.AllLiters = this.carsService.GetCurrentLitresByCarId(viewModel.CarId);
            viewModel.AllTravelledDistance = this.carsService.GetCurrentTravelledDistanceByCarId(viewModel.CarId, viewModel.Id);
            viewModel.AllFuelConsumption = this.carsService.GetCurrentFuelConsumptionByCarId(viewModel.CarId, viewModel.Id);
            viewModel.AllRoutes = this.routesService.GetAll();

            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TransportWortkTicketEditViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                var viewModel = await this.transportWorkTicketsService.GetDetailsAsync<TransportWortkTicketEditViewModel>(input.Id);

                viewModel.AllDrivers = this.usersService.GetUsersByCar(viewModel.CarId);
                viewModel.AllLiters = this.carsService.GetCurrentLitresByCarId(viewModel.CarId);
                viewModel.AllTravelledDistance = this.carsService.GetCurrentTravelledDistanceByCarId(viewModel.CarId, viewModel.Id);
                viewModel.AllFuelConsumption = this.carsService.GetCurrentFuelConsumptionByCarId(viewModel.CarId, viewModel.Id);
                viewModel.AllRoutes = this.routesService.GetAll();

                return this.View(viewModel);
            }

            await this.transportWorkTicketsService.EditAsync(input.Id, input.Date, input.ApplicationUserFullName, input.CarId, input.CarCompanyId, input.CreatedBy, input.CreatedOn, input.ModifiedBy, input.Route, input.StartKilometers, input.EndKilometers, input.FuelConsumption, input.Residue, input.FuelAvailability, input.TravelledDistance);

            return this.RedirectToAction("All", "TransportWorkTickets", new { id = input.CarId });
        }

        public async Task<ActionResult> RouteDetailsPartial(string id)
        {
            var viewModel = new TransportWorkTicketInputViewModel();
            viewModel.Routes = new List<RouteViewModel>();

            string[] splittedIds = id.Split(",", System.StringSplitOptions.RemoveEmptyEntries);

            foreach (var routeId in splittedIds)
            {
                var currentRoute = await this.routesService.GetByIdAsync<RouteViewModel>(routeId);
                viewModel.Routes.Add(currentRoute);
            }

            return this.PartialView("_RouteDetailsPartial", viewModel);
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
