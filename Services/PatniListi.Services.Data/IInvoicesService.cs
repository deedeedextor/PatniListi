namespace PatniListi.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IInvoicesService
    {
        IQueryable<T> GetAll<T>(string id);

        Task<T> GetByIdAsync<T>(string id);

        Task CreateAsync(string number, DateTime date, string location, decimal price, double quantity, string driver, string carId, string carCompanyId, string createdBy, string fuelType, decimal totalPrice);

        Task<T> GetDetailsAsync<T>(string id);

        Task EditAsync(string id, string number, DateTime date, string location, decimal price, double quantity, string driver, string carId, string carCompanyId, string createdBy, DateTime createdOn, string carFuelType, decimal totalPrice, string currentDriver);

        Task<bool> DeleteAsync(string id, string fullName);
    }
}
