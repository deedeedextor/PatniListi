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
    using PatniListi.Web.ViewModels.Models.Routes;

    public class RoutesService : IRoutesService
    {
        private const string InvalidRouteErrorMessage = "Не съществуващ маршрут.";

        private readonly IDeletableEntityRepository<Route> routesRepository;

        public RoutesService(IDeletableEntityRepository<Route> routesRepository)
        {
            this.routesRepository = routesRepository;
        }

        public async Task CreateAsync(RouteInputViewModel input)
        {
            var exists = this.IsExists(input);

            if (!exists)
            {
                var route = new Route
                {
                    StartPoint = input.StartPoint,
                    EndPoint = input.EndPoint,
                    Distance = input.Distance,
                };

                await this.routesRepository.AddAsync(route);
            }

            await this.routesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(RouteEditViewModel input)
        {
            var route = new Route
            {
                Id = input.Id,
                StartPoint = input.StartPoint,
                EndPoint = input.EndPoint,
                Distance = input.Distance,
                CreatedOn = input.CreatedOn,
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
                .FirstOrDefaultAsync();

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

        public bool IsExists(RouteInputViewModel input)
        {
            if (this.routesRepository.All().Any(r => r.StartPoint == input.StartPoint && r.EndPoint == input.EndPoint))
            {
                return true;
            }

            return false;
        }
    }
}
