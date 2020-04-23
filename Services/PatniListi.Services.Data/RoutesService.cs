namespace PatniListi.Services.Data
{
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

        public async Task EditAsync(Route route)
        {
            this.routesRepository.Update(route);
            await this.routesRepository.SaveChangesAsync();
        }

        public IQueryable<T> GetAll<T>()
        {
            return this.routesRepository
                .All()
                .OrderBy(r => r.StartPoint)
                .ThenBy(r => r.EndPoint)
                .To<T>();
        }

        public IEnumerable<SelectListItem> GetAll()
        {
            return this.routesRepository
                   .AllAsNoTracking()
                   .OrderBy(r => r.StartPoint)
                   .ThenBy(r => r.EndPoint)
                   .Select(r => new SelectListItem
                   {
                       Value = r.Id.ToString(),
                       Text = $"{r.StartPoint} - {r.EndPoint}",
                   })
                   .ToList();
        }

        public Route GetById(string id)
        {
            return this.routesRepository
                .AllAsNoTracking()
                .FirstOrDefault(r => r.Id == id);
        }

        public Task<T> GetByIdAsync<T>(string id)
        {
            var viewModel = this.routesRepository
                .AllAsNoTracking()
                .Where(r => r.Id == id)
                .To<T>()
                .SingleOrDefaultAsync();

            return viewModel;
        }

        public async Task<T> GetDetailsAsync<T>(string id)
        {
            var viewModel = await this.routesRepository
                .All()
                .Where(r => r.Id == id)
                .To<T>()
                .SingleOrDefaultAsync();

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
