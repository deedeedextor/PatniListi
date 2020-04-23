namespace PatniListi.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using PatniListi.Data.Models;
    using PatniListi.Web.ViewModels.Administration.Cars;

    public interface ICarsService
    {
        Car GetById(string id);

        IEnumerable<SelectListItem> GetFuelType();

        IEnumerable<SelectListItem> GetAll(string companyId);

        IEnumerable<SelectListItem> GetAllCarsByUserId(string userId, string companyId);

        IQueryable<T> GetAll<T>(string companyId);

        IQueryable<T> GetCarsByUser<T>(string userId, string companyId);

        Task CreateAsync(Car car);

        bool IsLicensePlateExist(string licensePlate);

        Task<T> GetDetailsAsync<T>(string id);

        Task EditAsync(Car car, string fullName);

        Task<bool> DeleteAsync(string id, string fullName);

        double GetCurrentLitresByCarId(string id);

        double GetCurrentTravelledDistanceByCarId(string carId, string transportWorkTicketId = null);

        double GetCurrentFuelConsumptionByCarId(string carId, string transportWorkTicketId = null);

        string GetLicensePlateById(string id);
    }
}
