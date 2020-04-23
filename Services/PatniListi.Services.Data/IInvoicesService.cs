namespace PatniListi.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IInvoicesService
    {
        IQueryable<T> GetAll<T>(string carId);

        IQueryable<T> GetAllInvoicesForPeriod<T>(string carId, DateTime from, DateTime to);

        Task CreateAsync(string number, DateTime date, string location, double currentLiters, decimal price, double quantity, string driver, string carId, string carCompanyId, string createdBy, string fuelType, decimal totalPrice);

        Task<T> GetDetailsAsync<T>(string id);

        Task EditAsync(string id, string number, DateTime date, string location, double currentLiters, decimal price, double quantity, string driver, string carId, string carCompanyId, string createdBy, DateTime createdOn, string modifiedBy, string carFuelType, decimal totalPrice);

        bool IsNumberExist(string number);

        string GetInvoiceNumberById(string id);
    }
}
