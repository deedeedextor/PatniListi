namespace PatniListi.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using PatniListi.Web.ViewModels.Models.Routes;

    public interface IRoutesService
    {
        IQueryable<T> GetAll<T>();

        Task CreateAsync(string startPoint, string endPoint, double distance);

        bool IsExists(string startPoint, string endPoint);

        Task<T> GetDetailsAsync<T>(string id);

        Task EditAsync(string id, string startPoint, string endPoint, double distance, DateTime createdOn);

        IEnumerable<SelectListItem> GetAll();

        Task<T> GetByIdAsync<T>(string id);

        Task<T> GetByNameAsync<T>(string startPoint, string EndPoint);
    }
}
