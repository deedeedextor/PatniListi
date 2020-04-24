namespace PatniListi.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PatniListi.Data.Models;

    public interface ITransportWorkTicketsService
    {
        public IQueryable<T> GetAll<T>(string id);

        IQueryable<T> GetAllTransportWorkTicketsForPeriod<T>(string carId, DateTime from, DateTime to);

        Task CreateAsync(TransportWorkTicket transportWorkTicket);

        Task<T> GetDetailsAsync<T>(string id);

        Task EditAsync(TransportWorkTicket transportWorkTicket);

        Task<bool> DeleteAsync(string id, string fullName);

        TransportWorkTicket GetById(string id);
    }
}
