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

        Task CreateAsync(Invoice invoice);

        Task<T> GetDetailsAsync<T>(string id);

        Task EditAsync(Invoice invoice);

        bool IsNumberExist(string number);

        string GetInvoiceNumberById(string id);

        Invoice GetById(string id);
    }
}
