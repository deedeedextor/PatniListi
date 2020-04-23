namespace PatniListi.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using PatniListi.Data.Models;

    public interface IRoutesService
    {
        Route GetById(string id);

        IQueryable<T> GetAll<T>();

        Task CreateAsync(string startPoint, string endPoint, double distance);

        bool IsExists(string startPoint, string endPoint);

        Task<T> GetDetailsAsync<T>(string id);

        Task EditAsync(Route route);

        IEnumerable<SelectListItem> GetAll();

        Task<T> GetByIdAsync<T>(string id);
    }
}
