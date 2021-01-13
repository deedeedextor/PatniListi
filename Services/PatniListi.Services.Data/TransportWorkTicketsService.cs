namespace PatniListi.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PatniListi.Data.Common.Repositories;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class TransportWorkTicketsService : ITransportWorkTicketsService
    {
        private readonly IDeletableEntityRepository<TransportWorkTicket> transportWorkTicketsRepository;

        public TransportWorkTicketsService(IDeletableEntityRepository<TransportWorkTicket> transportWorkTicketsRepository)
        {
            this.transportWorkTicketsRepository = transportWorkTicketsRepository;
        }

        public async Task<TransportWorkTicket> CreateAsync(DateTime date, string userId, string carId, string createdBy, double startKilometers, double endKilometers, double fuelConsumption, double residue, double fuelAvailability, double travelledDistance)
        {
            var transportWorkTicket = new TransportWorkTicket
            {
                Date = date,
                UserId = userId,
                CarId = carId,
                CreatedBy = createdBy,
                StartKilometers = startKilometers,
                EndKilometers = endKilometers,
                FuelConsumption = fuelConsumption,
                Residue = residue,
                FuelAvailability = fuelAvailability,
                TravelledDistance = travelledDistance,
            };

            await this.transportWorkTicketsRepository.AddAsync(transportWorkTicket);
            await this.transportWorkTicketsRepository.SaveChangesAsync();

            return transportWorkTicket;
        }

        public async Task<bool> DeleteAsync(string id, string fullName)
        {
            var transportWorkTicket = await this.transportWorkTicketsRepository
                   .All()
                   .Where(i => i.Id == id)
                   .SingleOrDefaultAsync();

            if (transportWorkTicket == null)
            {
                return false;
            }

            transportWorkTicket.ModifiedBy = fullName;

            this.transportWorkTicketsRepository.Delete(transportWorkTicket);
            await this.transportWorkTicketsRepository.SaveChangesAsync();

            return true;
        }

        public async Task EditAsync(string id, DateTime createdOn, DateTime date, string userId, string carId, string createdBy, string modifiedBy, double startKilometers, double endKilometers, double fuelConsumption, double residue, double fuelAvailability, double travelledDistance)
        {
            var transportWorkTicket = this.GetById(id);

            transportWorkTicket.CreatedOn = createdOn;
            transportWorkTicket.Date = date;
            transportWorkTicket.UserId = userId;
            transportWorkTicket.CarId = carId;
            transportWorkTicket.CreatedBy = createdBy;
            transportWorkTicket.ModifiedBy = modifiedBy;
            transportWorkTicket.StartKilometers = startKilometers;
            transportWorkTicket.EndKilometers = endKilometers;
            transportWorkTicket.FuelConsumption = fuelConsumption;
            transportWorkTicket.FuelAvailability = fuelAvailability;
            transportWorkTicket.Residue = residue;
            transportWorkTicket.TravelledDistance = travelledDistance;

            this.transportWorkTicketsRepository.Update(transportWorkTicket);
            await this.transportWorkTicketsRepository.SaveChangesAsync();
        }

        public IQueryable<T> GetAll<T>(string id)
        {
            return this.transportWorkTicketsRepository
                .All()
                .Where(c => c.CarId == id)
                .OrderBy(c => c.Date)
                .To<T>();
        }

        public IQueryable<T> GetAllTransportWorkTicketsForPeriod<T>(string carId, DateTime from, DateTime to)
        {
            return this.transportWorkTicketsRepository
                .AllAsNoTracking()
                .Where(tr => tr.CarId == carId && (tr.Date >= from && tr.Date <= to))
                .OrderBy(c => c.Date)
                .To<T>();
        }

        public async Task<T> GetDetailsAsync<T>(string id)
        {
            var viewModel = await this.transportWorkTicketsRepository
                .AllAsNoTracking()
                .Where(tr => tr.Id == id)
                .Include(tr => tr.RouteTransportWorkTickets)
                .To<T>()
                .FirstOrDefaultAsync();

            return viewModel;
        }

        public TransportWorkTicket GetById(string id)
        {
            return this.transportWorkTicketsRepository
                .AllAsNoTracking()
                .FirstOrDefault(c => c.Id == id);
        }
    }
}
