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
    using PatniListi.Web.ViewModels.Administration.Invoices;
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

        public async Task CreateAsync(InvoiceInputViewModel input)
        {
            var user = await this.usersService.GetByNameAsync<UserViewModel>(input.FullName, input.CarCompanyId);

            var invoice = new Invoice
            {
                Number = input.Number,
                Date = input.Date,
                FuelType = input.CarFuelType,
                Location = input.Location,
                Price = input.Price,
                Quantity = input.Quantity,
                TotalPrice = input.TotalPrice,
                UserId = user.Id,
                CarId = input.CarId,
                CreatedBy = input.CreatedBy,
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

            this.invoicesRepository.Delete(invoice);
            await this.invoicesRepository.SaveChangesAsync();

            return true;
        }

        public async Task EditAsync(InvoiceEditViewModel input, string fullName)
        {
            var user = await this.usersService.GetByNameAsync<UserViewModel>(input.FullName, input.CarCompanyId);

            var invoice = new Invoice
            {
                Id = input.Id,
                Number = input.Number,
                Date = input.Date,
                FuelType = input.CarFuelType,
                Location = input.Location,
                Price = input.Price,
                Quantity = input.Quantity,
                TotalPrice = input.TotalPrice,
                UserId = user.Id,
                CarId = input.CarId,
                CreatedBy = input.CreatedBy,
                CreatedOn = input.CreatedOn,
                ModifiedBy = fullName,
            };

            this.invoicesRepository.Update(invoice);
            await this.invoicesRepository.SaveChangesAsync();
        }

        public IQueryable<T> GetAll<T>(string id)
        {
            return this.invoicesRepository
                .All()
                .Where(c => c.CarId == id)
                .OrderByDescending(c => c.Date)
                .To<T>();
        }

        public IEnumerable<SelectListItem> GetAll(string companyId)
        {
            return this.usersService.GetAll(companyId);
        }

        public async Task<T> GetByIdAsync<T>(string id)
        {
            var viewModel = await this.invoicesRepository
                .All()
                .Where(c => c.CarId == id)
                .To<T>()
                .SingleOrDefaultAsync();

            if (viewModel == null)
            {
                throw new ArgumentNullException(InvalidInvoiceErrorMessage, id);
            }

            return viewModel;
        }

        public async Task<T> GetDetailsAsync<T>(string id)
        {
            var viewModel = await this.invoicesRepository
                .All()
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
