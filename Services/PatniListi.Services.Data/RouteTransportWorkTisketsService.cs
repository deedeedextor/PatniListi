﻿namespace PatniListi.Services.Data
{
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

        public async Task SetIsDeletedAsync(string transportWorkTicketId, string fullName)
        {
            var routeTransportWorkTicketsFromDb = await this.GetAllAsync<RouteTransportViewModel>(transportWorkTicketId);

            if (routeTransportWorkTicketsFromDb.Any())
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

        public async Task UpdateAsync(string transportWorkTicketId, string companyId, IEnumerable<string> collection)
        {
            var newRoutes = new List<RouteViewModel>();

            if (collection.Any())
            {
                foreach (var routeId in collection)
                {
                    var routeTransportWorkTicket = await this.routesService.GetByIdAsync<RouteViewModel>(routeId);
                    newRoutes.Add(routeTransportWorkTicket);
                }

                var allRoutes = await this.GetAllAsync<RouteTransportViewModel>(transportWorkTicketId);

                if (allRoutes.Any())
                {
                    foreach (var rtr in allRoutes)
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

                foreach (var route in newRoutes)
                {
                    var routeTransportWorkTicket = new RouteTransportWorkTicket
                    {
                        RouteId = route.Id,
                        TransportWorkTicketId = transportWorkTicketId,
                    };

                    await this.routeTransportWorkTicketsRepository.AddAsync(routeTransportWorkTicket);
                }

                await this.routeTransportWorkTicketsRepository.SaveChangesAsync();
            }
        }

        public async Task<List<T>> GetAllAsync<T>(string transportWorkTicketId)
        {
            return await this.routeTransportWorkTicketsRepository
                .All()
                .Where(rtr => rtr.TransportWorkTicketId == transportWorkTicketId)
                .To<T>()
                .ToListAsync();
        }
    }
}
