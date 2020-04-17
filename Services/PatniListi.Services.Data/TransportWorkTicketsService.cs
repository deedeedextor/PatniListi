namespace PatniListi.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PatniListi.Data.Common.Repositories;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;
    using PatniListi.Web.ViewModels.Administration.Users;
    using PatniListi.Web.ViewModels.Models.Routes;

    public class TransportWorkTicketsService : ITransportWorkTicketsService
    {
        private readonly IDeletableEntityRepository<TransportWorkTicket> transportWorkTicketsRepository;
        private readonly IUsersService usersService;
        private readonly IRouteTransportWorkTicketsService routeTransportWorkTicketsService;
        private readonly IRoutesService routesService;

        public TransportWorkTicketsService(IDeletableEntityRepository<TransportWorkTicket> transportWorkTicketsRepository, IUsersService usersService, IRouteTransportWorkTicketsService routeTransportWorkTicketsService, IRoutesService routesService)
        {
            this.transportWorkTicketsRepository = transportWorkTicketsRepository;
            this.usersService = usersService;
            this.routeTransportWorkTicketsService = routeTransportWorkTicketsService;
            this.routesService = routesService;
        }

        public async Task CreateAsync(DateTime date, string applicationUserFullName, string carId, string carCompanyId, string createdBy, IEnumerable<string> route, double startKilometers, double endKilometers, double fuelConsumption, double residue, double fuelAvailability, double travelledDistance)
        {
            var user = await this.usersService.GetByNameAsync<UserDetailsViewModel>(applicationUserFullName, carCompanyId);

            if (user == null)
            {
                return;
            }

            var transportWorkTicket = new TransportWorkTicket
            {
                Date = date,
                UserId = user.Id,
                CarId = carId,
                CreatedBy = createdBy,
                StartKilometers = startKilometers,
                EndKilometers = endKilometers,
                FuelConsumption = fuelConsumption,
                Residue = residue,
                FuelAvailability = fuelAvailability,
                TravelledDistance = travelledDistance,
            };

            await this.transportWorkTicketsRepository.AddAsync(transportWorkTicket);

            foreach (var routeId in route)
            {
                var currentRoute = await this.routesService.GetByIdAsync<RouteViewModel>(routeId);
                transportWorkTicket.RouteTransportWorkTickets.Add(new RouteTransportWorkTicket { RouteId = currentRoute.Id, TransportWorkTicketId = transportWorkTicket.Id });
            }

            await this.transportWorkTicketsRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(string id, string fullName)
        {
            var transportWorkTicket = await this.transportWorkTicketsRepository
                   .All()
                   .Where(i => i.Id == id)
                   .SingleOrDefaultAsync();

            if (transportWorkTicket == null)
            {
                return false;
            }

            transportWorkTicket.ModifiedBy = fullName;

            this.transportWorkTicketsRepository.Delete(transportWorkTicket);
            await this.routeTransportWorkTicketsService.SetIsDeletedAsync(transportWorkTicket.Id, fullName);
            await this.transportWorkTicketsRepository.SaveChangesAsync();

            return true;
        }

        public async Task EditAsync(string id, DateTime date, string applicationUserFullName, string carId, string carCompanyId, string createdBy, DateTime createdOn, string modifiedBy, IEnumerable<string> route, double startKilometers, double endKilometers, double fuelConsumption, double residue, double fuelAvailability, double travelledDistance)
        {
            var user = await this.usersService.GetByNameAsync<UserDetailsViewModel>(applicationUserFullName, carCompanyId);

            if (user == null)
            {
                return;
            }

            var transportWorkTicket = new TransportWorkTicket
            {
                Id = id,
                CreatedOn = createdOn,
                Date = date,
                UserId = user.Id,
                CarId = carId,
                CreatedBy = createdBy,
                ModifiedBy = modifiedBy,
                StartKilometers = startKilometers,
                EndKilometers = endKilometers,
                FuelConsumption = fuelConsumption,
                FuelAvailability = fuelAvailability,
                Residue = residue,
                TravelledDistance = travelledDistance,
            };

            await this.routeTransportWorkTicketsService.UpdateAsync(id, carCompanyId, route);

            this.transportWorkTicketsRepository.Update(transportWorkTicket);
            await this.transportWorkTicketsRepository.SaveChangesAsync();
        }

        public IQueryable<T> GetAll<T>(string id)
        {
            return this.transportWorkTicketsRepository
                .All()
                .Where(c => c.CarId == id)
                .OrderBy(c => c.Date)
                .To<T>();
        }

        public IQueryable<T> GetAllTransportWorkTicketsForPeriod<T>(string carId, DateTime from, DateTime to)
        {
            return this.transportWorkTicketsRepository
                .AllAsNoTracking()
                .Where(tr => tr.CarId == carId && (tr.Date >= from && tr.Date <= to))
                .OrderBy(c => c.Date)
                .To<T>();
        }

        public async Task<T> GetDetailsAsync<T>(string id)
        {
            var viewModel = await this.transportWorkTicketsRepository
                .All()
                .Where(tr => tr.Id == id)
                .Include(tr => tr.RouteTransportWorkTickets)
                .To<T>()
                .FirstOrDefaultAsync();

            return viewModel;
        }
    }
}
