using Microsoft.AspNetCore.Identity;

namespace PatniListi.Web.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using PatniListi.Common;
    using PatniListi.Data.Models;
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
        private readonly IRouteTransportWorkTicketsService routeTransportWorkTicketsService;
        private readonly UserManager<ApplicationUser> userManager;

        public TransportWorkTicketsController(ITransportWorkTicketsService transportWorkTicketsService, ICarsService carsService, IUsersService usersService, IRoutesService routesService, IRouteTransportWorkTicketsService routeTransportWorkTicketsService, UserManager<ApplicationUser> userManager)
        {
            this.transportWorkTicketsService = transportWorkTicketsService;
            this.carsService = carsService;
            this.usersService = usersService;
            this.routesService = routesService;
            this.routeTransportWorkTicketsService = routeTransportWorkTicketsService;
            this.userManager = userManager;
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

            var user = await this.userManager.GetUserAsync(this.User);

            var transportWorkTicket = new TransportWorkTicket
            {
                Date = input.Date,
                UserId = user.Id,
                CarId = input.CarId,
                CreatedBy = input.CreatedBy,
                StartKilometers = input.StartKilometers,
                EndKilometers = input.EndKilometers,
                FuelConsumption = input.FuelConsumption,
                Residue = input.Residue,
                FuelAvailability = input.FuelAvailability,
                TravelledDistance = input.TravelledDistance,
            };

            await this.transportWorkTicketsService.CreateAsync(transportWorkTicket);
            await this.routeTransportWorkTicketsService.UpdateAsync(transportWorkTicket.Id, input.CarCompanyId, input.Route);

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

            var transportWorkTicket = this.transportWorkTicketsService.GetById(input.Id);

            transportWorkTicket.CreatedOn = input.CreatedOn;
            transportWorkTicket.Date = input.Date;
            transportWorkTicket.UserId = input.ApplicationUserFullName;
            transportWorkTicket.CarId = input.CarId;
            transportWorkTicket.CreatedBy = input.CreatedBy;
            transportWorkTicket.ModifiedBy = input.ModifiedBy;
            transportWorkTicket.StartKilometers = input.StartKilometers;
            transportWorkTicket.EndKilometers = input.EndKilometers;
            transportWorkTicket.FuelConsumption = input.FuelConsumption;
            transportWorkTicket.FuelAvailability = input.FuelAvailability;
            transportWorkTicket.Residue = input.Residue;
            transportWorkTicket.TravelledDistance = input.TravelledDistance;

            await this.transportWorkTicketsService.EditAsync(transportWorkTicket);
            await this.routeTransportWorkTicketsService.UpdateAsync(transportWorkTicket.Id, input.CarCompanyId, input.Route);

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
    }
}
