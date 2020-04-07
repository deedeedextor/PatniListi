namespace PatniListi.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PatniListi.Data.Common.Repositories;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;
    using PatniListi.Web.ViewModels.Administration.TransportWorkTickets;
    using PatniListi.Web.ViewModels.Administration.Users;
    using PatniListi.Web.ViewModels.Models.Routes;

    public class TransportWorkTicketsService : ITransportWorkTicketsService
    {
        private const string InvalidRouteErrorMessage = "Не съществуващ пътен лист.";

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

        public async Task CreateAsync(TransportWorkTicketInputViewModel input)
        {
            var user = await this.usersService.GetByNameAsync<UserDetailsViewModel>(input.ApplicationUserFullName, input.CarCompanyId);

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

            await this.transportWorkTicketsRepository.AddAsync(transportWorkTicket);

            foreach (var routeId in input.Route)
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

        public async Task EditAsync(TransportWortkTicketEditViewModel input, string fullName)
        {
            var user = await this.usersService.GetByNameAsync<UserDetailsViewModel>(input.ApplicationUserFullName, input.CarCompanyId);

            var transportWorkTicket = new TransportWorkTicket
            {
                Id = input.Id,
                CreatedOn = input.CreatedOn,
                Date = input.Date,
                UserId = user.Id,
                CarId = input.CarId,
                CreatedBy = input.CreatedBy,
                ModifiedBy = input.ModifiedBy,
                StartKilometers = input.StartKilometers,
                EndKilometers = input.EndKilometers,
                FuelConsumption = input.FuelConsumption,
                FuelAvailability = input.FuelAvailability,
                Residue = input.Residue,
                TravelledDistance = input.TravelledDistance,
            };

            await this.routeTransportWorkTicketsService.UpdateAsync(input.Id, input.CarCompanyId, input.Route);

            this.transportWorkTicketsRepository.Update(transportWorkTicket);
            await this.transportWorkTicketsRepository.SaveChangesAsync();
        }

        public IQueryable<T> GetAll<T>(string id)
        {
            return this.transportWorkTicketsRepository
                .All()
                .Where(c => c.CarId == id)
                .OrderByDescending(c => c.Date)
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

            if (viewModel == null)
            {
                throw new ArgumentNullException(InvalidRouteErrorMessage);
            }

            return viewModel;
        }
    }
}
