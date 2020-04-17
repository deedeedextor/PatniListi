namespace PatniListi.Services.Data
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using PatniListi.Data.Common.Repositories;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;
    using PatniListi.Web.ViewModels.Administration.Users;

    public class InvoicesService : IInvoicesService
    {
        private readonly IDeletableEntityRepository<Invoice> invoicesRepository;
        private readonly IUsersService usersService;

        public InvoicesService(IDeletableEntityRepository<Invoice> invoicesRepository, IUsersService usersService)
        {
            this.invoicesRepository = invoicesRepository;
            this.usersService = usersService;
        }

        public async Task CreateAsync(string number, DateTime date, string location, double currentLiters, decimal price, double quantity, string driver, string carId, string carCompanyId, string createdBy, string fuelType, decimal totalPrice)
        {
            var user = await this.usersService.GetByNameAsync<UserViewModel>(driver, carCompanyId);

            if (user == null)
            {
                return;
            }

            var invoice = new Invoice
            {
                Number = number,
                Date = date,
                FuelType = fuelType,
                Location = location,
                CurrentLiters = currentLiters,
                Price = price,
                Quantity = quantity,
                TotalPrice = totalPrice,
                UserId = user.Id,
                CarId = carId,
                CreatedBy = createdBy,
            };

            await this.invoicesRepository.AddAsync(invoice);
            await this.invoicesRepository.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(string id, string fullName)
        {
            var invoice = await this.invoicesRepository
                .All()
                .Where(i => i.Id == id)
                .SingleOrDefaultAsync();

            if (invoice == null)
            {
                return false;
            }

            invoice.ModifiedBy = fullName;
            this.invoicesRepository.Delete(invoice);
            await this.invoicesRepository.SaveChangesAsync();

            return true;
        }

        public async Task EditAsync(string id, string number, DateTime date, string location, double currentLiters, decimal price, double quantity, string driver, string carId, string carCompanyId, string createdBy, DateTime createdOn, string modifiedBy, string carFuelType, decimal totalPrice)
        {
            var user = await this.usersService.GetByNameAsync<UserViewModel>(driver, carCompanyId);

            if (user == null)
            {
                return;
            }

            var invoice = new Invoice
            {
                Id = id,
                Number = number,
                Date = date,
                FuelType = carFuelType,
                Location = location,
                CurrentLiters = currentLiters,
                Price = price,
                Quantity = quantity,
                TotalPrice = totalPrice,
                UserId = user.Id,
                CarId = carId,
                CreatedBy = createdBy,
                CreatedOn = createdOn,
                ModifiedBy = modifiedBy,
            };

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
    }
}
