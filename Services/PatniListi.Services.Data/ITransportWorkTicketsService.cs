namespace PatniListi.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using PatniListi.Web.ViewModels.Administration.TransportWorkTickets;

    public interface ITransportWorkTicketsService
    {
        public IQueryable<T> GetAll<T>(string id);

        IQueryable<T> GetAllTransportWorkTicketsForPeriod<T>(string carId, DateTime from, DateTime to);

        Task CreateAsync(DateTime date, string applicationUserFullName, string carId, string carCompanyId, string createdBy, IEnumerable<string> route, double startKilometers, double endKilometers, double fuelConsumption, double residue, double fuelAvailability, double travelledDistance);

        Task<T> GetDetailsAsync<T>(string id);

        Task EditAsync(string id, DateTime date, string applicationUserFullName, string carId, string carCompanyId, string createdBy, DateTime createdOn, string modifiedBy, IEnumerable<string> route, double startKilometers, double endKilometers, double fuelConsumption, double residue, double fuelAvailability, double travelledDistance);

        Task<bool> DeleteAsync(string id, string fullName);
    }
}
