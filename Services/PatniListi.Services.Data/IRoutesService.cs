namespace PatniListi.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using PatniListi.Web.ViewModels.Models.Routes;

    public interface IRoutesService
    {
        IQueryable<T> GetAll<T>();

        Task CreateAsync(RouteInputViewModel input);

        bool IsExists(RouteInputViewModel input);

        Task<T> GetDetailsAsync<T>(string id);

        Task EditAsync(RouteEditViewModel input);

        IEnumerable<SelectListItem> GetAll();

        Task<T> GetByIdAsync<T>(string id);
    }
}
