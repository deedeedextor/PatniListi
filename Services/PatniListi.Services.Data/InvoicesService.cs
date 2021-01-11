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

        public async Task CreateAsync(string number, DateTime date, string carFuelType, string location, double currentLiters, decimal price, double quantity, decimal totalPrice, string userId, string carId, string createdBy)
        {
            var invoice = new Invoice
            {
                Number = number,
                Date = date,
                FuelType = carFuelType,
                Location = location,
                CurrentLiters = currentLiters,
                Price = price,
                Quantity = quantity,
                TotalPrice = totalPrice,
                UserId = userId,
                CarId = carId,
                CreatedBy = createdBy,
            };

            await this.invoicesRepository.AddAsync(invoice);
            await this.invoicesRepository.SaveChangesAsync();
        }

        public async Task EditAsync(string id, string number, DateTime date, string carFuelType, string location, double currentLiters, decimal price, double quantity, decimal totalPrice, string userId, string carId, string createdBy, DateTime createdOn, string modifiedBy)
        {
            var invoice = this.GetById(id);

            invoice.Number = number;
            invoice.Date = date;
            invoice.FuelType = carFuelType;
            invoice.Location = location;
            invoice.CurrentLiters = currentLiters;
            invoice.Price = price;
            invoice.Quantity = quantity;
            invoice.TotalPrice = totalPrice;
            invoice.UserId = userId;
            invoice.CarId = carId;
            invoice.CreatedBy = createdBy;
            invoice.CreatedOn = createdOn;
            invoice.ModifiedBy = modifiedBy;

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
