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

        public async Task CreateAsync(TransportWorkTicket transportWorkTicket)
        {
            await this.transportWorkTicketsRepository.AddAsync(transportWorkTicket);
            await this.transportWorkTicketsRepository.SaveChangesAsync();
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

        public async Task EditAsync(TransportWorkTicket transportWorkTicket)
        {
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
