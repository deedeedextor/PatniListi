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
    using PatniListi.Web.ViewModels.Models.Routes;

    public class RouteTransportWorkTisketsService : IRouteTransportWorkTicketsService
    {
        private readonly IDeletableEntityRepository<RouteTransportWorkTicket> routeTransportWorkTicketsRepository;
        private readonly IRoutesService routesService;

        public RouteTransportWorkTisketsService(IDeletableEntityRepository<RouteTransportWorkTicket> routeTransportWorkTicketsRepository, IRoutesService routesService)
        {
            this.routeTransportWorkTicketsRepository = routeTransportWorkTicketsRepository;
            this.routesService = routesService;
        }

        public async Task<T> GetByIdAsync<T>(string id)
        {
            return await this.routeTransportWorkTicketsRepository
                .AllAsNoTracking()
                .Where(rtr => rtr.TransportWorkTicketId == id)
                .To<T>()
                .SingleOrDefaultAsync();
        }

        public async Task SetIsDeletedAsync(string id, string fullName)
        {
            var routeTransportWorkTicketsFromDb = await this.GetAllAsync<RouteTransportViewModel>(id);

            if (routeTransportWorkTicketsFromDb != null)
            {
                foreach (var rtr in routeTransportWorkTicketsFromDb)
                {
                    var routeTransportWorkTicket = new RouteTransportWorkTicket
                    {
                        TransportWorkTicketId = rtr.TransportWorkTicketId,
                        RouteId = rtr.RouteId,
                    };

                    routeTransportWorkTicket.ModifiedBy = fullName;
                    this.routeTransportWorkTicketsRepository.Delete(routeTransportWorkTicket);

                    await this.routeTransportWorkTicketsRepository.SaveChangesAsync();
                }
            }
        }

        public async Task UpdateAsync(string transportWorkTicketId, string companyId, IEnumerable<RouteTransportViewModel> collection)
        {
            if (collection.Count() > 0)
            {
                var routeTransportWorkTickets = await this.GetAllAsync<RouteTransportViewModel>(transportWorkTicketId);

                if (routeTransportWorkTickets.Count() > 0)
                {
                    foreach (var rtr in routeTransportWorkTickets)
                    {
                        var routeTransportWorkTicket = new RouteTransportWorkTicket
                        {
                            TransportWorkTicketId = rtr.TransportWorkTicketId,
                            RouteId = rtr.RouteId,
                        };

                        this.routeTransportWorkTicketsRepository.HardDelete(routeTransportWorkTicket);

                        await this.routeTransportWorkTicketsRepository.SaveChangesAsync();
                    }
                }

                foreach (var route in collection)
                {
                    var routeTransportWorkTicket = new RouteTransportWorkTicket
                    {
                        RouteId = route.RouteId,
                        TransportWorkTicketId = transportWorkTicketId,
                    };

                    await this.routeTransportWorkTicketsRepository.AddAsync(routeTransportWorkTicket);
                }

                await this.routeTransportWorkTicketsRepository.SaveChangesAsync();
            }
        }

        public async Task<List<T>> GetAllAsync<T>(string id)
        {
            return await this.routeTransportWorkTicketsRepository
                .All()
                .Where(rtr => rtr.TransportWorkTicketId == id)
                .To<T>()
                .ToListAsync();
        }
    }
}
