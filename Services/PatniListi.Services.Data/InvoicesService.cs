namespace PatniListi.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PatniListi.Data.Common.Repositories;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;

    public class InvoicesService : IInvoicesService
    {
        private readonly IDeletableEntityRepository<Invoice> invoicesRepository;

        public InvoicesService(IDeletableEntityRepository<Invoice> invoicesRepository)
        {
            this.invoicesRepository = invoicesRepository;
        }

        public async Task CreateAsync(Invoice invoice)
        {
            await this.invoicesRepository.AddAsync(invoice);
            await this.invoicesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(Invoice invoice)
        {

            this.invoicesRepository.Update(invoice);
            await this.invoicesRepository.SaveChangesAsync();
        }

        public IQueryable<T> GetAll<T>(string carId)
        {
            return this.invoicesRepository
                .AllAsNoTracking()
                .Where(c => c.CarId == carId)
                .OrderBy(c => c.Date)
                .To<T>();
        }

        public IQueryable<T> GetAllInvoicesForPeriod<T>(string carId, DateTime from, DateTime to)
        {
            return this.invoicesRepository
                .AllAsNoTracking()
                .Where(c => c.CarId == carId && (c.Date >= from && c.Date <= to))
                .OrderBy(c => c.Date)
                .To<T>();
        }

        public async Task<T> GetDetailsAsync<T>(string id)
        {
            var viewModel = await this.invoicesRepository
                .AllAsNoTracking()
                .Where(c => c.Id == id)
                .To<T>()
                .SingleOrDefaultAsync();

            return viewModel;
        }

        public string GetInvoiceNumberById(string id)
        {
            return this.invoicesRepository
                .AllAsNoTracking()
                .Where(i => i.Id == id)
                .Select(i => i.Number)
                .SingleOrDefault();
        }

        public bool IsNumberExist(string number)
        {
            var exists = this.invoicesRepository
                .AllAsNoTracking()
                .Any(c => c.Number == number);

            if (exists)
            {
                return true;
            }

            return false;
        }

        public Invoice GetById(string id)
        {
            return this.invoicesRepository
                .AllAsNoTracking()
                .FirstOrDefault(c => c.Id == id);
        }
    }
}
