namespace PatniListi.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using PatniListi.Web.ViewModels.Administration.Cars;

    public interface ICarsService
    {
        Task<T> GetByIdAsync<T>(string carId);

        IEnumerable<SelectListItem> GetFuelType();

        IEnumerable<SelectListItem> GetAll(string companyId);

        IQueryable<T> GetAll<T>(string companyId);

        IQueryable<T> GetCarsByUser<T>(string userId, string companyId);

        Task CreateAsync(CarInputViewModel input);

        Task<T> GetDetailsAsync<T>(string id);

        Task EditAsync(CarEditViewModel input, string fullName);

        Task<bool> DeleteAsync(string id, string fullName);

        double GetCurrentLitresByCarId(string id);

        double GetCurrentTravelledDistanceByCarId(string id);

        double GetCurrentFuelConsumptionByCarId(string id);
    }
}
