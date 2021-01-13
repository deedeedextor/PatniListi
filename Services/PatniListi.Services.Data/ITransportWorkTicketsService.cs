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

        Task<TransportWorkTicket> CreateAsync(DateTime date, string userId, string carId, string createdBy, double startKilometers, double endKilometers, double fuelConsumption, double residue, double fuelAvailability, double travelledDistance);

        Task<T> GetDetailsAsync<T>(string id);

        Task EditAsync(string id, DateTime createdOn, DateTime date, string userId, string carId, string createdBy, string modifiedBy, double startKilometers, double endKilometers, double fuelConsumption, double residue, double fuelAvailability, double travelledDistance);

        Task<bool> DeleteAsync(string id, string fullName);

        TransportWorkTicket GetById(string id);
    }
}
