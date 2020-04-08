namespace PatniListi.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.EntityFrameworkCore;
    using PatniListi.Data.Common.Repositories;
    using PatniListi.Data.Models;
    using PatniListi.Services.Mapping;
    using PatniListi.Web.ViewModels.Administration.Users;

    public class InvoicesService : IInvoicesService
    {
        private const string InvalidInvoiceErrorMessage = "Не съществуваща фактура.";

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

        public async Task EditAsync(string id, string number, DateTime date, string location, double currentLiters, decimal price, double quantity, string driver, string carId, string carCompanyId, string createdBy, DateTime createdOn, string carFuelType, decimal totalPrice, string currentDriver)
        {
            var user = await this.usersService.GetByNameAsync<UserViewModel>(driver, carCompanyId);

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
                ModifiedBy = currentDriver,
            };

            this.invoicesRepository.Update(invoice);
            await this.invoicesRepository.SaveChangesAsync();
        }

        public IQueryable<T> GetAll<T>(string carId)
        {
            return this.invoicesRepository
                .AllAsNoTracking()
                .Where(c => c.CarId == carId)
                .OrderByDescending(c => c.Date)
                .To<T>();
        }

        public IEnumerable<SelectListItem> GetAll(string companyId)
        {
            return this.usersService.GetAll(companyId);
        }

        public async Task<T> GetByIdAsync<T>(string carId)
        {
            var viewModel = await this.invoicesRepository
                .All()
                .Where(c => c.CarId == carId)
                .To<T>()
                .SingleOrDefaultAsync();

            if (viewModel == null)
            {
                throw new ArgumentNullException(InvalidInvoiceErrorMessage, carId);
            }

            return viewModel;
        }

        public async Task<T> GetDetailsAsync<T>(string id)
        {
            var viewModel = await this.invoicesRepository
                .AllAsNoTracking()
                .Where(c => c.Id == id)
                .To<T>()
                .SingleOrDefaultAsync();

            if (viewModel == null)
            {
                throw new ArgumentNullException(InvalidInvoiceErrorMessage, id);
            }

            return viewModel;
        }
    }
}
