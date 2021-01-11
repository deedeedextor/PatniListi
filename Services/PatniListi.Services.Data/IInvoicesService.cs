namespace PatniListi.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using PatniListi.Data.Models;

    public interface IInvoicesService
    {
        IQueryable<T> GetAll<T>(string carId);

        IQueryable<T> GetAllInvoicesForPeriod<T>(string carId, DateTime from, DateTime to);

        Task CreateAsync(string number, DateTime date, string carFuelType, string location, double currentLiters, decimal price, double quantity, decimal totalPrice, string userId, string carId, string createdBy);

        Task<T> GetDetailsAsync<T>(string id);

        Task EditAsync(string id, string number, DateTime date, string carFuelType, string location, double currentLiters, decimal price, double quantity, decimal totalPrice, string userId, string carId, string createdBy, DateTime createdOn, string modifiedBy);

        bool IsNumberExist(string number);

        string GetInvoiceNumberById(string id);

        Invoice GetById(string id);
    }
}
