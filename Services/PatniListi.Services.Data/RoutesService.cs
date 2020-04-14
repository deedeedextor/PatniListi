namespace PatniListi.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using PatniListi.Data.Common.Repositories;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class RoutesService : IRoutesService
    {
        private const string InvalidRouteErrorMessage = "Не съществуващ маршрут.";

        private readonly IDeletableEntityRepository<Route> routesRepository;

        public RoutesService(IDeletableEntityRepository<Route> routesRepository)
        {
            this.routesRepository = routesRepository;
        }

        public async Task CreateAsync(string startPoint, string endPoint, double distance)
        {
            var exists = this.IsExists(startPoint, endPoint);

            if (!exists)
            {
                var route = new Route
                {
                    StartPoint = startPoint,
                    EndPoint = endPoint,
                    Distance = distance,
                };

                await this.routesRepository.AddAsync(route);
            }

            await this.routesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(string id, string startPoint, string endPoint, double distance, DateTime createdOn)
        {
            var route = new Route
            {
                Id = id,
                StartPoint = startPoint,
                EndPoint = endPoint,
                Distance = distance,
                CreatedOn = createdOn,
            };

            this.routesRepository.Update(route);
            await this.routesRepository.SaveChangesAsync();
        }

        public IQueryable<T> GetAll<T>()
        {
            return this.routesRepository
                .All()
                .To<T>();
        }

        public IEnumerable<SelectListItem> GetAll()
        {
            return this.routesRepository
                   .AllAsNoTracking()
                   .Select(r => new SelectListItem
                   {
                       Value = r.Id.ToString(),
                       Text = $"{r.StartPoint} - {r.EndPoint}",
                   })
                   .ToList();
        }

        public Task<T> GetByIdAsync<T>(string id)
        {
            var viewModel = this.routesRepository
                .AllAsNoTracking()
                .Where(r => r.Id == id)
                .To<T>()
                .SingleOrDefaultAsync();

            if (viewModel == null)
            {
                throw new ArgumentNullException(InvalidRouteErrorMessage);
            }

            return viewModel;
        }

        public async Task<T> GetByNameAsync<T>(string startPoint, string endPoint)
        {
            var viewModel = await this.routesRepository
                .AllAsNoTracking()
                .Where(r => r.StartPoint == startPoint && r.EndPoint == endPoint)
                .To<T>()
                .SingleOrDefaultAsync();

            if (viewModel == null)
            {
                throw new ArgumentNullException(InvalidRouteErrorMessage);
            }

            return viewModel;
        }

        public async Task<T> GetDetailsAsync<T>(string id)
        {
            var viewModel = await this.routesRepository
                .All()
                .Where(r => r.Id == id)
                .To<T>()
                .SingleOrDefaultAsync();

            if (viewModel == null)
            {
                throw new ArgumentNullException(InvalidRouteErrorMessage);
            }

            return viewModel;
        }

        public bool IsExists(string startPoint, string endPoint)
        {
            if (this.routesRepository.All().Any(r => r.StartPoint == startPoint && r.EndPoint == endPoint))
            {
                return true;
            }

            return false;
        }
    }
}
