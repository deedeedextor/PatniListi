namespace PatniListi.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using PatniListi.Web.ViewModels.Administration.Cars;

    public interface ICarsService
    {
        IEnumerable<SelectListItem> GetFuelType();

        IQueryable<T> GetAll<T>(string companyId);

        Task CreateAsync(CarInputViewModel input);

        Task<T> GetDetailsAsync<T>(string id);

        Task EditAsync(CarEditViewModel input);

        Task<bool> DeleteAsync(string id);
    }
}
